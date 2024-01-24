using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetX, offsetZ;
    [SerializeField] private float LerpSpeed;

    //Lerp: 선형보간함수.
    //찾고싶은 일부 지점을 비율로 표시해서 값을 예측해서 받는다.
    
    private void LateUpdate() 
        //모든 Update 함수가 호출된 후 마지막으로 호출된다
        //특히 플레이어를 따라가는 카메라는 LateUpdate를 쓴다.
    {
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.position.x + offsetX,
                        300, //y축을 고정
                        target.position.z + offsetZ), LerpSpeed);
    }
}
