using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    private void Start()
    {
        //OverlapSphere : �ֺ��� �ִ� ��ü �ľ��ϱ�
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.01f);

        //foreach���� �� �� �ݺ��� ������ ������ �迭�� ��Ҹ� ���ʴ�� ��ȸ�ϸ鼭
        //in Ű���� ���� ������ ��´�.
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Wall")
            {
                Destroy(gameObject);
                return;
            }
        }

        GetComponent<Collider>().enabled = true;
    }
}
