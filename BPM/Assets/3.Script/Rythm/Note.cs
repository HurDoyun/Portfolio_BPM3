using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{ 
    public float speed = 400f; //noteSpeed
    public Image image;
    
    private void OnEnable() //활성화 될때마다 호출되는 함수
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }

        image.enabled = true;
    }

    private void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Center"))
        {
            StartCoroutine(ChangeColor());
        }
    }
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.1f);
        image.color = new Color(1, 1, 0, 1f);

        yield return new WaitForSeconds(0.1f);
        image.color = new Color(1, 1, 1, 0.5f);
    }
}
