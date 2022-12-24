using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun",menuName ="Create New Gun")]

public class Gun : ScriptableObject
{
    public float fireRate;
    public float damage;
    public int magazine;
    public bool unLimited;
    public bool defaultGun;

    public Transform bulletSpawnPoint;
    public GameObject gunPreFab;
    public GameObject MuzzleFlash;
    

}
