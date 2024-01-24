using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn : MonoBehaviour
{
    private void Update()
    {
        MoveScene();
    }
    public void MoveScene()
    {
        SceneManager.LoadScene(4);
    }
}
