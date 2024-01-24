using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteInfo
{
    public GameObject goPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class NotePooling : MonoBehaviour
{
    [SerializeField] NoteInfo[] objInfo = null;
    public static NotePooling instance = null;
    public Queue<GameObject> noteQueue = new Queue<GameObject>();

    private void Start()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        noteQueue = InsertQueue(objInfo[1]);
    }

    Queue<GameObject> InsertQueue(NoteInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();

        for (int i = 0; i < p_objectInfo.count; i++)
        {
            GameObject t_clone =
                Instantiate(p_objectInfo.goPrefab, transform.position, Quaternion.identity);

            t_clone.SetActive(false);

            if (p_objectInfo.tfPoolParent != null)
            {
                t_clone.transform.SetParent(p_objectInfo.tfPoolParent);
            }
            else
            {
                t_clone.transform.SetParent(this.transform);
            }

            t_queue.Enqueue(t_clone); //반복문이 전부 돌면 queue에는 count 개수만큼의 객체가 들어간다. 
        }

        return t_queue;
    }

}
