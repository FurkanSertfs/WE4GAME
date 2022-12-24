using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed;

    public float damage;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up *speed, ForceMode2D.Impulse);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamageable>()!=null)
        {
            collision.GetComponent<IDamageable>().TakeHit(damage);

            Destroy(gameObject);
        }
    }
}
