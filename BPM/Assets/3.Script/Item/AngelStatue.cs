using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelStatue : MonoBehaviour
{
    [SerializeField] private ParticleSystem holyEff;
    [SerializeField] private PlayerMove player;
    [SerializeField] private Coin coin;

    public float radius = 5f;
    private bool canFkey = false;
    private bool canPay = true;

    [SerializeField]
    private string Angelbless;
    [SerializeField]
    private string Donation;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        coin = FindObjectOfType<Coin>();
        holyEff.Stop();
    }
    private void Update()
    {
        CheckPlayer();
    }
    private void CheckPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player") && player.wallet > 0 && canPay)
            {
                Debug.Log("player °¨Áö");
                canFkey = true;
                Donate();

                break;
            }
            
        }
    }

    private void Donate()
    {
        if (canFkey && Input.GetKeyDown(KeyCode.F))
        {
            //coinPrefab.SetActive(true);
            coin.CoinAppear();

            player.wallet--;
            canFkey = false;
            canPay = false;
            Debug.Log("µ·³»±â");

            AudioManager.instance.PlaySE(Donation);

            Invoke("CallStats", 0.4f);
            EffectOn();
        }
    }

    private void EffectOn()
    {
        holyEff.Play();
        AudioManager.instance.PlaySE(Angelbless);
    }
    private void CallStats()
    {
        player.StatsChange();
    }

}
