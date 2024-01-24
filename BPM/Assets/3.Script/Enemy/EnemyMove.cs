using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public enum Type
    {
        normal, rusher, thrower, boss
        //일반형, 돌격형, 원거리형, 보스
    }
    public Type enemytype;
    public float maxHp;
    public float curHp;
    public float damage;
    public float throwdistance;

    [SerializeField] public Transform target;
    public NavMeshAgent nav; // NavMesh 필요함. NavMesh : NavAgent가 경로를 그리기 위한 바탕

    public Rigidbody rb;
    [SerializeField] public BoxCollider meleeArea;
    //public MeshRenderer[] meshs;
    public Animator anim;

    [SerializeField] public GunShot gunshot;

    private bool isChase;
    private bool isAtk;

    [SerializeField] public ParticleSystem bloodEff;
    [SerializeField] public GameObject arrowObj;
    [SerializeField] public GameObject throwEnemy;

    private bool flash = false;
    public bool isShield = false;

    private void Start() //Awake함수는 상속이 안됨
    {
        rb = GetComponent<Rigidbody>();
        meleeArea = GetComponent<BoxCollider>();
        //meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        gunshot = FindObjectOfType<GunShot>();

        curHp = maxHp;

        if (enemytype != Type.boss)
        {
            Invoke("ChaseStart", 2f);
        }

    }
    private void Update()
    {
        //SetDestination : 도착할 목표 위치 지정함수
        if (nav.enabled && enemytype != Type.boss)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }

        //피격 이펙트
        if (flash)
        {
            bloodEff.Play();
        }
        else
        {
            bloodEff.Stop();
        }
    }
    private void FixedUpdate()
    {
        FreezeVelocity();
        Targeting();
    }

    private void Targeting()
    {
        
            float targetRadius = 0f;
            float targetRange = 0f;

            switch (enemytype)
            {
                case Type.normal:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;

                case Type.rusher:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;

                case Type.thrower:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;

                case Type.boss:
                    targetRadius = 100f;
                    targetRange = 25f;
                    break;

            }

            RaycastHit[] rayhits =
            Physics.SphereCastAll(transform.position,
                                  targetRadius,
                                  transform.forward,
                                  targetRange,
                                  LayerMask.GetMask("Player"));

        if(enemytype != Type.boss)
        {
            if (rayhits.Length > 0 && !isAtk)
            {
                StartCoroutine(Attack());
            }
        }
        
    }

    private void FreezeVelocity() //골드메탈 3d 쿼터뷰 참고
    {
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    private void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            
            if(enemytype != Type.boss) //보스는 개별적으로 OnDamage 시스템을 갖춤
            {
                curHp -= gunshot.damage;
                StartCoroutine(OnDamage());
            }
        }

        if (enemytype != Type.boss)
        {
            if (other.CompareTag("Player"))
            {
                anim.SetBool("isAtk", true);
            }
            else
            {
                anim.SetBool("isAtk", false);
            }
        }
    }
    private void OnDie()
    {
        Destroy(this.gameObject);
    }

    IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(0.1f);

        if (curHp > 0)
        {
            flash = true;

            yield return new WaitForSeconds(0.3f);
            flash = false;
        }
        else
        {
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("Die");
            Invoke("OnDie", 1f);

            flash = true;

            yield return new WaitForSeconds(0.6f);
            flash = false;
        }
    }

    public IEnumerator Attack()
    {
        isChase = false;
        nav.enabled = false;

        if(enemytype != Type.boss)
        {
            isAtk = true;
            anim.SetBool("isAtk", true);
        }
        
        switch (enemytype)
        {
            case Type.normal:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                meleeArea.enabled = true; //이렇게 한 이유 있다

                yield return new WaitForSeconds(1f);
                break;

            case Type.rusher:
                yield return new WaitForSeconds(0.1f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                meleeArea.enabled = false;
                meleeArea.enabled = true; //이렇게 한 이유 있다

                yield return new WaitForSeconds(1f);
                break;

            case Type.thrower:
            case Type.boss:
                yield return new WaitForSeconds(0.1f);
                Vector3 arrowPos =
                    new Vector3(throwEnemy.transform.position.x,
                                5f,
                                throwEnemy.transform.position.z);

                GameObject instantArrow =
                    Instantiate(arrowObj, arrowPos, Quaternion.identity);
                Rigidbody rd_arrow = instantArrow.GetComponent<Rigidbody>();
                rd_arrow.velocity = transform.forward * throwdistance;

                yield return new WaitForSeconds(2f);
                break;

        }

        isChase = true;
        nav.enabled = true;

        if (enemytype != Type.boss)
        {
            isAtk = false;
            anim.SetBool("isAtk", false);
        }
    }
}
