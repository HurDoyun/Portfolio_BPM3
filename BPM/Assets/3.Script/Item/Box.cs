using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject text3d;
    [SerializeField] private List<GameObject> items = new List<GameObject>();

    private Animator anim;
    public float radius = 10f;
    public bool canOpen = false;
    public bool Notext = false;

    [SerializeField]
    private string Open;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        CheckPlayer();

        if (Input.GetKeyDown(KeyCode.F) && canOpen) //������ �ٰ����� �����ϰ� �����
        {
            OpenBox();
            text3d.SetActive(false);
        }
    }
    private void CheckPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player") && !Notext)
            {
                //Debug.Log("�浹_Player");
                text3d.SetActive(true);
                canOpen = true;
                break;
            }
            else
            {
                text3d.SetActive(false);
                canOpen = false;
            }
        }

    }
    private void OpenBox()
    {
        anim.SetTrigger("Openbox");

        AudioManager.instance.PlaySE(Open);

        Notext = true;
        //�������� �������� ����
        RandomItem();
    }
    private void RandomItem()
    {
        int randomItem = Random.Range(0, items.Count);
        Debug.Log("���������� : " + items[randomItem].name);

        items[randomItem].SetActive(true);
    }
}
