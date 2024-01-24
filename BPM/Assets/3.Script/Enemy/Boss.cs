using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : EnemyMove
{

    [SerializeField] private Slider hpbar;
    private float recoveryRate = 1f; //ȸ���ӵ�

    private Vector3 lookVec; //�÷��̾� ������ ���� ���� ���� ����
    //private Vector3 tauntVec;
    private bool isLook = true; //�÷��̾� �ٶ󺸴� �÷��� bool����
    public bool isDead = false;

    [SerializeField] private ParticleSystem dieEff;
    [SerializeField] GameObject head;
    [SerializeField] GameObject eye1;
    [SerializeField] GameObject eye2;

    [SerializeField] GameObject bomb;
    [SerializeField] private BomberRange bomberRange;
    [SerializeField] GameObject Shield;

    [Header("Sound")]
    [SerializeField] private string Boss_Dead;
    [SerializeField] private string Whoosh;
    [SerializeField] private string Boss_Heal;

    IEnumerator random_spawn;
    IEnumerator healself;

    private void Awake()
    {
        bomberRange = FindObjectOfType<BomberRange>();
        hpbar.value = (float)curHp / (float)maxHp;
        dieEff.Stop();

        StartCoroutine(Think());

        random_spawn = RandomSpawn();
        healself = HealSelf();
    }

    private void Update()
    {
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }

        HpBar();
        AllStop();

    }

    private void AllStop()
    {
        if (isDead)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator Think()
    {
        if (isDead)
        {
            Invoke("AllStop", 0.2f);
            yield break;
        }

        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 3); // 012

        switch (ranAction)
        {
            case 0:
                StartCoroutine(Healing());
                break;

            case 1:
                StartCoroutine(FireShot());
                break;

            case 2:
                StartCoroutine(PrayMeteo());
                break;
          
        }

    }

    private IEnumerator Healing() //����0
    {
        yield return new WaitForSeconds(0.1f);
        isLook = false;
        anim.SetBool("doSpread", true);
        StartCoroutine(healself);
        AudioManager.instance.PlaySE(Boss_Heal);

        isShield = true;
        Shield.SetActive(true);

        yield return new WaitForSeconds(5f);
        anim.SetBool("doSpread", false);
        isLook = true;
        StopCoroutine(healself);

        isShield = false;
        Shield.SetActive(false);
        AudioManager.instance.StopSE(Boss_Heal);

        StartCoroutine(Think());
    }

    private IEnumerator FireShot() //���̾ 1
    {
        anim.SetTrigger("doThrow");

        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.PlaySE(Whoosh);
        StartCoroutine(Attack());

        yield return new WaitForSeconds(2f);
        AudioManager.instance.StopSE(Whoosh);
        StartCoroutine(Think());
    }

    private IEnumerator Rush() //���� 2
    {
        anim.SetTrigger("doRush");

        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }

    private IEnumerator PrayMeteo() //�⵵�� ū ���̾ ����
    {
        isLook = false;
        anim.SetTrigger("isPraying");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(random_spawn);

        yield return new WaitForSeconds(2f);
        isLook = true;
        StopCoroutine(random_spawn);
        StartCoroutine(Think());
    }

    private IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(0.1f);

        if (curHp <= 0)
        {
            //isChase = false;
            nav.enabled = false;
            isDead = true;
            isLook = false;
            gameObject.tag = "bossDie";
            anim.SetTrigger("Die");

            AudioManager.instance.PlaySE(Boss_Dead);

            head.SetActive(false);
            eye1.SetActive(false);
            eye2.SetActive(false);

            Shield.SetActive(false);

            dieEff.Play();
        }
    }

    private IEnumerator RandomSpawn() //ū ���̾ ����
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject instantBomb = Instantiate(bomb, bomberRange.Return_RandomPos(), Quaternion.identity);
        }
    }

    private IEnumerator HealSelf()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / recoveryRate);
            
            if (curHp < maxHp)
            {
                curHp++;
                
            }
            
        }
    }

    private void HpBar()
    {
        hpbar.value = (float)curHp / (float)maxHp;
    }
    private void OnTriggerEnter(Collider bossbody)
    {
        if (bossbody.CompareTag("Bullet") && !isShield)
        {
            curHp -= gunshot.damage;
            StartCoroutine(OnDamage());
        }
    }
}
