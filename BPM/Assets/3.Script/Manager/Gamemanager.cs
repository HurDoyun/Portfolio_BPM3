using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    //timescale - tabŰ ���� �� timescale = 0
    //���� ��� ���Ͱ� �� ������ box�� ��Ÿ�� + ���� ����

    //[SerializeField] private GameObject box;
    [SerializeField] private PlayerMove player;

    private bool startGame = false;
    private bool roomClear = false;

    [Header("BGM")]
    [SerializeField] private string FirstFloor;
    [SerializeField] private string Die;

    [SerializeField] GameObject menus;

    private int pressESC = 0;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>();
        AudioManager.instance.PlayBGM(FirstFloor);
    }

    private void Update()
    {
        MyInput();

        if (player.curHp <= 0)
        {
            Invoke("MoveScene", 0.2f);
        }
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (pressESC == 0 || pressESC % 2 == 1))
        {
            pressESC++;

            if (pressESC == 0 || pressESC % 2 == 1)
            {
                Time.timeScale = 0;
                menus.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pressESC = 0;
                menus.SetActive(false);
            }
        }
    }

    private void Appear_box()
    {
        if (roomClear)
        {
            //�����ڽ� appear
            //�� open
        }
    }

    private void FloorStart()
    {
        //�� ��������
        //�̴ϸ� ����
    }

    private void RoomStart()
    {
        //�� close
        //���� ���� ���� (���� ��, ���� ����)
        //õ�絿�� 1
    }

    private void BossStart()
    {
        //���� �׸� �����
        //�� close

        if (roomClear)
        {
            //�����ڽ� 3�� appear
            //�� open
            //���� ������ ���� ��Ż Appear
        }
    }

    private void MoveScene()
    {
        AudioManager.instance.PlayBGM(Die);
        SceneManager.LoadScene(4); //����
    }
}
