using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //�� ������� ����
    /*
     ������ �¾��� ��
     ���� �¾��� ��
     �ٴڿ� �¾��� ��
     */
    
    private void OnTriggerEnter(Collider other)
    {
        bool touch = other.CompareTag("Ground") || 
                     other.CompareTag("Wall") ||
                     other.CompareTag("Enemy");

        if (touch)
        {
            OnDestroy();
        }
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
