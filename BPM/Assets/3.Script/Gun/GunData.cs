using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    public new string name;
    public GameObject prefab;
    public float reloadTime, damage, fireRate, shootForce, dirForce; //fireRate : 연사속도
    public float maxDisance; 
    public float magazineSize, bulletsPerTap;
    public float spread;
    //public int price;
    //public AudioClip fire_sound;
}
