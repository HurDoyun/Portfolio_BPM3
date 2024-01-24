using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    private void Start()
    {
        //OverlapSphere : 주변에 있는 물체 파악하기
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.01f);

        //foreach문이 한 번 반복을 수행할 때마다 배열의 요소를 차례대로 순회하면서
        //in 키워드 앞의 변수에 담는다.
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
