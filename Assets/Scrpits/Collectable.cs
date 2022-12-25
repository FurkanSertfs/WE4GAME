using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour, ICollectable
{
    public PlayerController.CollectibleObject collectableType;

    public void Collect(PlayerController playerController)
    {
        for (int i = 0; i < playerController.collectables.Count; i++)
        {
            if (playerController.collectables[i].collectableObjects == collectableType)
            {
                playerController.collectables[i].count++;
                Destroy(gameObject);
            }
        }
    }
}
