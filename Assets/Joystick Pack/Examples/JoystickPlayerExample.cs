﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deathmatch.io.Packets;
public class JoystickPlayerExample : MonoBehaviour
{
    [SerializeField]

    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody2D rigidbody;
    public Vector2 move;

    

    private void Update()
    {
        float hAxis = move.x;
        float vAxis = move.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        if (zAxis!=0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, -zAxis);

        }

    }

    public void FixedUpdate()
    {
        move.x = variableJoystick.Horizontal;
        move.y = variableJoystick.Vertical;


        rigidbody.position = (rigidbody.position + move.normalized * speed * Time.deltaTime);

        if (NetworkManager.instance.server != null)
        {
            NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new ClientMovementPacket
            {
                PositionX = transform.position.x,
                PositionY = transform.position.y,
                PositionZ = transform.position.z,
                Rotation = transform.eulerAngles.z

            }, LiteNetLib.DeliveryMethod.ReliableOrdered);
        }
        else
        {
            Debug.Log("Server yok haci");
        }



    }
}