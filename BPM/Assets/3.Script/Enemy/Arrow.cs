using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        bool touch = other.CompareTag("Player") ||
                     other.CompareTag("Wall") ||
                     other.CompareTag("Ground");

        if (touch)
        {
            Destroy(this.gameObject);
        }
    }
}
