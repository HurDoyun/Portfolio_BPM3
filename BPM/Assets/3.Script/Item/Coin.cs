using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject coinPrefab;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    public void CoinAppear()
    {
        anim.SetTrigger("CoinFall");
        Invoke("OnDestroy", 0.2f);
    }
    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
    
}
