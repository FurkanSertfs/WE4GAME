using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Probs : MonoBehaviour, IDamageable
{
    [SerializeField]
    float health;

    [SerializeField]
    Transform dropObjectSpawnPoint;

    [SerializeField]
    GameObject dropObject;

    public int id;

    public void TakeHit(float damage)
    {
        health -= damage;

        transform.DOShakeRotation(0.15f,10,10);
        

        if (health<=0)
        {
            Instantiate(dropObject, dropObjectSpawnPoint.position, dropObjectSpawnPoint.rotation);
            Destroy(gameObject);
        }

    }
}
