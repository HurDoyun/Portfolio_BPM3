using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetX, offsetZ;
    [SerializeField] private float LerpSpeed;

    //Lerp: ���������Լ�.
    //ã����� �Ϻ� ������ ������ ǥ���ؼ� ���� �����ؼ� �޴´�.
    
    private void LateUpdate() 
        //��� Update �Լ��� ȣ��� �� ���������� ȣ��ȴ�
        //Ư�� �÷��̾ ���󰡴� ī�޶�� LateUpdate�� ����.
    {
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.position.x + offsetX,
                        300, //y���� ����
                        target.position.z + offsetZ), LerpSpeed);
    }
}
