using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    //timescale - tab키 누를 때 timescale = 0
    //방의 모든 몬스터가 다 죽으면 box가 나타남 + 문이 열림

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
            //랜덤박스 appear
            //문 open
        }
    }

    private void FloorStart()
    {
        //맵 랜덤생성
        //미니맵 생성
    }

    private void RoomStart()
    {
        //문 close
        //몬스터 랜덤 생성 (몬스터 수, 종류 랜덤)
        //천사동상 1
    }

    private void BossStart()
    {
        //보스 테마 오디오
        //문 close

        if (roomClear)
        {
            //랜덤박스 3개 appear
            //문 open
            //다음 층으로 가는 포탈 Appear
        }
    }

    private void MoveScene()
    {
        AudioManager.instance.PlayBGM(Die);
        SceneManager.LoadScene(4); //죽음
    }
}
