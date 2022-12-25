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
    public GameObject dropObject1;
    public GameObject dropObject2;
    public GameObject dropObject3;

    public int id;

    public void TakeHit(float damage)
    {
        health -= damage;

        transform.DOShakeRotation(0.15f,10,10);
        

        if (health<=0)
        {
            Destroy(gameObject);
            float randomChance = Random.Range(0, 100f);
            if (randomChance <= 25)
            {
                Instantiate(dropObject1, dropObjectSpawnPoint.position, dropObjectSpawnPoint.rotation);
            }
            else if (randomChance < 50)
            {
                Instantiate(dropObject2, transform.position, transform.rotation);


            }
            else if (randomChance < 100)
            {
                Instantiate(dropObject3, transform.position, transform.rotation);
            }



        }

    }
}
