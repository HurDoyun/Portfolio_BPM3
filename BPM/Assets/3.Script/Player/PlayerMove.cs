using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    public float cameraRotLimit;
    private float currentCamRot_X = 0f; // 0은 정면
    public float lookSense; //민감도

    private Rigidbody rd;

    public float speed = 100f;

    [Header(" ")]
    //Jump
    public float jumpForce = 3f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GunAnimation gunAni;

    private Vector3 velocity;

    //Dash
    //private bool dashing = true;
    public float dashingPower = 20f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 0.75f;

    [Header(" ")]
    //HP
    [SerializeField] private Slider hpBar;
    [SerializeField] private Text hpText;
    public float curHp;
    public float maxHp = 100f;

    [Header(" ")]
    [SerializeField] GameObject hitEffect;
    public bool isDamage = false;

    [SerializeField] private GunShot gunshot;

    public int wallet = 0;
    [SerializeField] private Text wallet_text;

    [SerializeField] private GameObject statsUI;

    private int pressTab = 0;

    [Header("대쉬 이미지")]
    [SerializeField] GameObject DashImage;

    [Header("UI 텍스트")]
    [SerializeField] private Text Damage_txt;
    [SerializeField] private Text DelayShot_txt;
    [SerializeField] private Text Speed_txt;
    [SerializeField] private Text Bullet_txt;

    private EnemyMove enemy;
    private Arrow arrow;
    private CenterFrame center;

    [Header("Sound")]
    [SerializeField] private string AbilityUp;
    [SerializeField] private string Land;
    [SerializeField] private string GetCoin;
    [SerializeField] private string GreenPotion;
    [SerializeField] private string RedPotion;
    [SerializeField] private string Hurt;
    [SerializeField] private string Portal;

    private float rotationSpeed = 5f;

    [SerializeField] GameObject box;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; //커서 잠그기

        rd = GetComponent<Rigidbody>();

        gunAni = FindObjectOfType<GunAnimation>();
        gunshot = FindObjectOfType<GunShot>();
        enemy = FindObjectOfType<EnemyMove>();
        arrow = FindObjectOfType<Arrow>();
        center = FindObjectOfType<CenterFrame>();
        
        curHp = maxHp;
        hpBar.value = (float)curHp / (float)maxHp;

        gunshot.gunData.magazineSize = 8f;
        Bullet_txt.text = $"Bullet : {gunshot.gunData.magazineSize}";
    }
    private void Update()
    {
        Move();
        CameraRot();
        CharacterRotation();
        HpUI();
        StatsOn();
        UpdateStats();

        wallet_text.text = " " + wallet;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            box.SetActive(true);
        }

    }

    private void StatsOn()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && (pressTab == 0 || pressTab % 2 == 1))
        {
            pressTab++;

            if (pressTab == 0 || pressTab % 2 == 1)
            {
                statsUI.SetActive(true);
            }
            else
            {
                pressTab = 0;
                statsUI.SetActive(false);
            }
        }
        //else if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    pressTab = 0;
        //    statsUI.SetActive(false);
        //}

    }

    private void UpdateStats()
    {
        if (gunshot != null)
        {
            //스탯창
            Damage_txt.text = $"Damage : {gunshot.damage}";
            DelayShot_txt.text = $"DelayShot : {gunshot.timeBetweenShooting}";
            Speed_txt.text = $"Speed : {speed}";
            Bullet_txt.text = $"Bullet : {gunshot.gunData.magazineSize}";
        }
        else
        {
            //gundata가 없을 경우에 대한 예외 처리
            Debug.LogWarning("GunShot이 설정되지 않았습니다.");
        }
    }

    private void HpUI()
    {
        hpText.text = (float)curHp + " / " + (float)maxHp;
        hpBar.value = (float)curHp / (float)maxHp;

        if (curHp <= 0)
        {
            curHp = 0;
            StartCoroutine(RotateX());
        }
        else if (curHp >= maxHp)
        {
            curHp = maxHp;
        }

    }

    private void CharacterRotation()
    {
        //좌우 캐릭터 회전
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRot_Y = new Vector3(0, yRotation, 0f) * lookSense;
        rd.MoveRotation(rd.rotation * Quaternion.Euler(characterRot_Y));
    }

    private void CameraRot()
    {
        //상하 카메라 회전
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotX = xRotation * lookSense;
        currentCamRot_X -= cameraRotX;
        currentCamRot_X = Mathf.Clamp(currentCamRot_X, -cameraRotLimit, cameraRotLimit);
        camera.transform.localEulerAngles = new Vector3(currentCamRot_X, 0, 0);
    }

    private void Move()
    {
        //isGrounded = Physics.Raycast(groundCheck.position, Vector3.down);

        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
        rd.MovePosition(transform.position + velocity * Time.deltaTime);

        //점프
        if (Input.GetButtonDown("Jump") && IsGrounded() && center.inputRythm)
        {
            AudioManager.instance.PlaySE(Land);
            rd.velocity = new Vector3(rd.velocity.x, jumpForce, rd.velocity.z);
        }

        ////4방향 대쉬 - 기술문서
        //if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift)) && 
        //    (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W)) && center.inputRythm)
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && center.inputRythm)
        {
            StartCoroutine(FowardDash());
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && center.inputRythm)
        {
            StartCoroutine(RightDash());
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S) && center.inputRythm)
        {
            StartCoroutine(BackDash());
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) && center.inputRythm)
        {
            StartCoroutine(LeftDash());
        }
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
    }

    private IEnumerator FowardDash()
    {
        //dashing = true;
        DashImage.SetActive(true);

        rd.velocity = new Vector3(transform.forward.x * dashingPower,
            0f, transform.forward.z * dashingPower);

        yield return new WaitForSeconds(dashingTime);
        DashImage.SetActive(false);
        rd.velocity = Vector3.zero;
    }
    private IEnumerator RightDash()
    {
        DashImage.SetActive(true);

        rd.velocity = new Vector3(transform.right.x * dashingPower,
            0f, transform.right.z * dashingPower);

        yield return new WaitForSeconds(dashingTime);
        DashImage.SetActive(false);
        rd.velocity = Vector3.zero;
    }
    private IEnumerator BackDash()
    {
        DashImage.SetActive(true);

        rd.velocity = new Vector3(-(transform.forward.x * dashingPower),
            0f, -(transform.forward.z * dashingPower));

        yield return new WaitForSeconds(dashingTime);
        DashImage.SetActive(false);
        rd.velocity = Vector3.zero;
    }
    private IEnumerator LeftDash()
    {
        DashImage.SetActive(true);

        rd.velocity = new Vector3(-(transform.right.x * dashingPower),
            0f, -(transform.right.z * dashingPower));

        yield return new WaitForSeconds(dashingTime);

        rd.velocity = Vector3.zero;
        yield return new WaitForSeconds(dashingCooldown);
    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Enemy":
                StartCoroutine(OnDamage());
                break;

            case "EnemyBullet":
                StartCoroutine(DamagedArrow());
                AudioManager.instance.PlaySE(Hurt);
                break;

            case "Coin":
                AudioManager.instance.PlaySE(GetCoin);

                Destroy(col.gameObject);
                wallet++;
                wallet_text.text = " " + wallet;
                break;

            case "Redpotion":
                AudioManager.instance.PlaySE(RedPotion);
                Destroy(col.gameObject);

                if (curHp < maxHp)
                {
                    Destroy(col.gameObject);
                    curHp += 10;
                    Debug.Log("curHp : " + curHp);
                }
                //curHp = maxHp일 때 포션이 더 이상 안먹어지게 수정하기
                break;

            case "Greenpotion":
                AudioManager.instance.PlaySE(GreenPotion);
                Destroy(col.gameObject);
                maxHp += 25;
                Debug.Log("maxHp : " + maxHp);
                break;

            case "Ability":
                Destroy(col.gameObject);
                RandomAbility();
                break;

            case "Portal_toBoss":
                AudioManager.instance.PlaySE(Portal);
                GoBossRoom();
                break;
            
        }
    }

    private void GoBossRoom()
    {
        SceneManager.LoadScene("BossRoom");
    }
    
    private void RandomAbility()
    {
        AudioManager.instance.PlaySE(AbilityUp);

        //랜덤으로 한 능력이 오름
        int randomNum = Random.Range(0, 4);

        switch (randomNum)
        {
            case 0:
                gunshot.damage += 5f;
                Debug.Log("Damage change : " + gunshot.damage);
                Damage_txt.color = Color.yellow;
                Invoke("ChangeOriginColor", 3f);
                break;

            case 1:
                gunshot.timeBetweenShooting -= 0.1f;
                Debug.Log("DelayShot change : " + gunshot.timeBetweenShooting);
                DelayShot_txt.color = Color.yellow;
                Invoke("ChangeOriginColor", 3f);
                break;

            case 2:
                speed += 100f;
                Debug.Log("Speed change : " + speed);
                Speed_txt.color = Color.yellow;
                Invoke("ChangeOriginColor", 3f);
                break;

            case 3:
                gunshot.gunData.magazineSize += 1f;
                Debug.Log("Bullet change : " + gunshot.gunData.magazineSize);
                Bullet_txt.color = Color.yellow;
                Invoke("ChangeOriginColor", 3f);
                break;
        }

    }

    private IEnumerator OnDamage()
    {
        if (!isDamage)
        {
            AudioManager.instance.PlaySE(Hurt);

            curHp -= enemy.damage;
            hitEffect.SetActive(true);
            isDamage = true;
        }

        yield return new WaitForSeconds(1f);
        isDamage = false;
        hitEffect.SetActive(false);
    }
    
    private IEnumerator DamagedArrow()
    {
        if (!isDamage)
        {
            AudioManager.instance.PlaySE(Hurt);

            curHp -= arrow.damage;
            hitEffect.SetActive(true);
            isDamage = true;
        }

        yield return new WaitForSeconds(1f);
        isDamage = false;
        hitEffect.SetActive(false);
    }

    private IEnumerator DamagedBomb()
    {
        if (!isDamage)
        {
            AudioManager.instance.PlaySE(Hurt);

            curHp -= 5;
            hitEffect.SetActive(true);
            isDamage = true;
        }

        yield return new WaitForSeconds(1f);
        isDamage = false;
        hitEffect.SetActive(false);
    }

    private IEnumerator RotateX()
    {
        //모두 멈춤(스케일 0)
        float targetRotation = 90f;

        while(transform.eulerAngles.x < targetRotation)
        {
            float newRotation =
                Mathf.MoveTowardsAngle(transform.eulerAngles.x, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(newRotation, transform.eulerAngles.y, transform.eulerAngles.z);

            yield return null;
        }

        transform.eulerAngles = new Vector3(targetRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    public void StatsChange()
    {
        statsUI.SetActive(true);
        RandomAbility();
    }

    private void ChangeOriginColor()
    {
        Damage_txt.color = Color.white;
        DelayShot_txt.color = Color.white;
        Speed_txt.color = Color.white;
        Bullet_txt.color = Color.white;

        pressTab = 0;
        statsUI.SetActive(false);
    }

    public void BombHit()
    {
        StartCoroutine(DamagedBomb());
    }

}
