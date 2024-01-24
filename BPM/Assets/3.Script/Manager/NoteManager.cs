using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currTime = 0d; //currentTime, float���� ������ �� ���� double

    [SerializeField] Transform NoteAppear; //tfNoteAppear, note ������ ��ġ

    [SerializeField] private Note noteScript;
    private void Start()
    {
        noteScript = FindObjectOfType<Note>();
    }
    private void Update()
    {
        currTime += Time.deltaTime;

        if (currTime >= 60 / bpm)
        //60/bpm : 1��Ʈ�� ���� �ӵ�
        {
            GameObject t_note = NotePooling.instance.noteQueue.Dequeue();
            t_note.transform.position = NoteAppear.position;
            t_note.SetActive(true);

            currTime -= 60d / bpm; //�Ҽ��� ������ ���ֱ� ���� ���� ��
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
