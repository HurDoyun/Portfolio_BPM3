using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //총 사라지는 조건
    /*
     적에게 맞았을 때
     벽에 맞았을 때
     바닥에 맞았을 때
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
