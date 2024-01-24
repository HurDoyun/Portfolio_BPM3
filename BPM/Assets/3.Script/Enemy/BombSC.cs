using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSC : MonoBehaviour
{
    //터지는 순간 Enemy tag로 바뀜

    [SerializeField] GameObject bombEff;
    
    [Header("Sound")]
    [SerializeField] private string FireBomb;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Ground")
        {
            StartCoroutine(Explosion());
        }
    }

    private IEnumerator Explosion()
    {
        AudioManager.instance.PlaySE(FireBomb);

        yield return new WaitForSeconds(0.1f);
        bombEff.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.StopSE(FireBomb);
        Destroy(this.gameObject);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                     15,
                                                     Vector3.up, 0f,
                                                     LayerMask.GetMask("Player"));
        foreach(RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PlayerMove>().BombHit();
        }
    }
}
