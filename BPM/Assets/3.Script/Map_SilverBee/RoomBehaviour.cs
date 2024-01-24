using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] walls; // 0-up, 1-down, 2-right, 3-left
    [SerializeField] private GameObject[] doors;
    //[SerializeField] private GameObject[] miniTile;
    private void Start()
    {
        //UpdateRoom(testStatus);
    }
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++) //0,1
        {
            //door가 true일 땐 wall가 false, door가 false일 땐 wall가 true
            //wall이 false면 minimapTile은 true
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }

    }
    
}
