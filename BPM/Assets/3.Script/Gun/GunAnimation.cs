using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    public Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void ReloadAnim()
    {
        anim.SetTrigger("Reloading");
    }
    public void GobackIdle()
    {
        anim.SetTrigger("Idle");
    }
    public void RecoilAni()
    {
        anim.SetTrigger("isShot");
    }
}
