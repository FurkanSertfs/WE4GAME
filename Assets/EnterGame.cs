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

    public void EnterGameButton()
    {


        if (NetworkManager.instance.server != null)
        {


            NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new ClientJoinRequestPacket
            {
                Username = playerName

            }, LiteNetLib.DeliveryMethod.ReliableOrdered);
        }
    }

    public void OnNameChange()
    {
        playerName = inputField.text;
    }
}
