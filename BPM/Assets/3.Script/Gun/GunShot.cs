using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunShot : MonoBehaviour
{
    [Header("무기 데이터")]
    public GunData gunData;

    [Header("총쏘는데 필요한 데이터")]
    // bulletsLeft : 남은 탄, currentAmmo : 현재 내가 들고있는 무기
    //public int currentAmmo;
    public float bulletsLeft;
    public int bulletsShot;
    public float timeBetweenShooting;
    public float timeBetweenShots;
    public bool allowButtonHold;
    private bool shooting, readyToShoot, reloading;

    //bullet
    [SerializeField] private GameObject bullet;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //bug fixing
    public bool allowInvoke = true;

    [SerializeField] private GunAnimation gunAni;
    [SerializeField] private Text bulletText;

    public float damage;

    [SerializeField] private ParticleSystem particleObj;
    public bool playFlash; //파티클 제어

    [SerializeField] private CenterFrame center;

    //Sound
    [SerializeField]
    private string Shot;
    [SerializeField]
    private string Reload;
    [SerializeField]
    private string Buzzer;

    public void Set(GunData gundata) //GunData 가져오기
    {
        this.gunData = gundata;
        bulletsLeft = gundata.magazineSize;
        this.damage = gundata.damage;
    }
    private void Start()
    {
        //make sure magazine is full
        Set(gunData);
        readyToShoot = true;
        gunAni = FindObjectOfType<GunAnimation>();
        center = FindObjectOfType<CenterFrame>();
        bulletsLeft = 8f; //이거 없으면 게임 새로 시작할 때 text가 8로 초기화가 안됨
    }
    private void Update()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        bulletText.text = ("Bullet : " + bulletsLeft);

        if (playFlash)
        {
            particleObj.Play();
        }
        else
        {
            particleObj.Stop();
        }
    }
    private void FixedUpdate()
    {
        MyInput();
    }
    
    public void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0) && center.inputRythm;
        }

        //Reloading
        if (Input.GetKey(KeyCode.R) &&
            bulletsLeft < gunData.magazineSize &&
            !reloading &&
            center.inputRythm)
        {
            Reloading(); 
        }

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            gunAni.RecoilAni();
            Shoot();
            playFlash = true;
        }
        else if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) //탄창이 비었는데 쏠 때
        {
            AudioManager.instance.PlaySE(Buzzer);
        }
        else
        {
            playFlash = false;
        }
    }

    public void Shoot()
    {
        AudioManager.instance.PlaySE(Shot);

        readyToShoot = false;

        #region 총알을 사용할 경우
        //레이캐스트를 이용하여 정확한 적중 위치 찾기
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); //화면 정중앙 좌표
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit)) //기억 안나면 문법 참고
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75); //just a point far away from the player
        }

        //Calculate direction from attackPoint to targetPoint 
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-gunData.spread, gunData.spread);
        float y = Random.Range(-gunData.spread, gunData.spread);

        //Calculate new direction with spread 퍼지는 방향을 계산. 식이 왜 이런지는 그려보면 앎
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instatiate bullet / projectile
        //currentBullet : 스폰된 총알들은 여기에 저장됨
        GameObject currentBullet =
            Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().
            AddForce(directionWithSpread.normalized * gunData.shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().
            AddForce(fpsCam.transform.forward * gunData.shootForce, ForceMode.Impulse);

        #endregion

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function(if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletPerTap make sure to repeat shoot function
        if (bulletsShot < gunData.bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

    }
    
    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
    public void Reloading()
    {
        AudioManager.instance.PlaySE(Reload);

        reloading = true;
        gunAni.ReloadAnim();
        Invoke("ReloadFin", gunData.reloadTime);
    }
    private void ReloadFin() //Shooting
    {
        bulletsLeft = gunData.magazineSize;
        gunAni.GobackIdle();
        reloading = false;
    }
}
