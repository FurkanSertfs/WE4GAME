using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Deathmatch.io.Packets;

public class EnterGame : MonoBehaviour
{

    [SerializeField]
    InputField inputField;

    public string playerName;

    bool isPressed;

    public void EnterGameButton()
    {


        if (NetworkManager.instance.server != null)
        {
            if (!isPressed)
            {
                isPressed = true;
                NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new ClientJoinRequestPacket
                {
                    Username = playerName

                }, LiteNetLib.DeliveryMethod.ReliableOrdered);
            }

           
        }
    }

    public void OnNameChange()
    {
        playerName = inputField.text;
    }
}
