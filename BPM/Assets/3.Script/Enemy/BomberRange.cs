using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberRange : MonoBehaviour
{
    [SerializeField] GameObject rangeObj;
    private BoxCollider rangeCol;
    

    private void Awake()
    {
        rangeCol = rangeObj.GetComponent<BoxCollider>();
    }

    public Vector3 Return_RandomPos()
    {
        Vector3 originPos = rangeObj.transform.position;

        //콜라이더의 사이즈를 가져오는 bound.size 사용
        float rangeX = rangeCol.bounds.size.x;
        float rangeZ = rangeCol.bounds.size.z;

        rangeX = Random.Range((rangeX / 2) * -1, rangeX / 2);
        rangeZ = Random.Range((rangeZ / 2) * -1, rangeZ / 2);
        Vector3 randomPos = new Vector3(rangeX, 10f, rangeZ);

        Vector3 respawnPos = originPos + randomPos;
        return respawnPos;
    }
}
