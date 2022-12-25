using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ranks : MonoBehaviour
{
    [SerializeField] Text[] ranksText; 

    private void Start()
    {
        if (NetworkManager.instance!=null)
        {
            ranksText[0].text = "1. " + NetworkManager.instance.ranks[0];
            ranksText[1].text = "2. " + NetworkManager.instance.ranks[0];
            ranksText[2].text = "3. " + NetworkManager.instance.ranks[0];

        }

    }

}
