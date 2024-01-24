using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterFrame : MonoBehaviour
{
    //[SerializeField] new AudioSource audio; //myAudio
    Image image;
    private bool musicStart = false;
    public bool inputRythm = false; 

    private void Start()
    {
        //audio = GetComponent<AudioSource>();
        image = GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Note") && !musicStart)
        {
            musicStart = true;
        }

        if (col.CompareTag("Note"))
        {
            StartCoroutine(ChangeColor());
        }

    }
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.1f);
        image.color = new Color(1, 1, 1, 1f);
        inputRythm = true;

        yield return new WaitForSeconds(0.1f);
        image.color = new Color(1, 1, 1, 0.5f);
        inputRythm = false;
    }
}
