using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currTime = 0d; //currentTime, float보다 오차가 더 적은 double

    [SerializeField] Transform NoteAppear; //tfNoteAppear, note 생성될 위치

    [SerializeField] private Note noteScript;
    private void Start()
    {
        noteScript = FindObjectOfType<Note>();
    }
    private void Update()
    {
        currTime += Time.deltaTime;

        if (currTime >= 60 / bpm)
        //60/bpm : 1비트당 등장 속도
        {
            GameObject t_note = NotePooling.instance.noteQueue.Dequeue();
            t_note.transform.position = NoteAppear.position;
            t_note.SetActive(true);

            currTime -= 60d / bpm; //소수점 오차를 없애기 위해 넣은 식
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Note"))
        {
            NotePooling.instance.noteQueue.Enqueue(col.gameObject);
            col.gameObject.SetActive(false);
        }
    }
}
