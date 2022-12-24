using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up *speed, ForceMode2D.Impulse);

       

    }
}
