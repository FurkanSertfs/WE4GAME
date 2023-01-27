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

            for (int i = 0; i < NetworkManager.instance.ranks.Length; i++)
            {
                ranksText[0].text += i+1+" " + NetworkManager.instance.ranks[i]+"\n";

            }

           
           

        }

    }

}
