using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deathmatch.io.Packets;

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

            if (NetworkManager.instance.server != null)
            {
                NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new PlayerHitPacket
                {
                    receiverId = collision.GetComponentInParent<Probs>().id,
                    receivedDamage = damage,
                }, LiteNetLib.DeliveryMethod.ReliableOrdered); 

                Destroy(gameObject);
            }


           
        }

        if (collision.GetComponent<PlayerController>()!=null)
        {
            if (NetworkManager.instance.server != null)
            {
                NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new PlayerHitPacket
                {
                   receiverId = collision.GetComponentInParent<ClientPlayer>().Id,
                   receivedDamage = damage,
                }, LiteNetLib.DeliveryMethod.ReliableOrdered);

                Destroy(gameObject);
            }

        }
    }
}
