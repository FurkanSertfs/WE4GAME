using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LobyCountDown : MonoBehaviour
{

    bool isStart;

    private void Update()
    {
        if(!isStart && NetworkManager.instance.lobyCountDown!=-1)
        {
            isStart = true;


            StartCoroutine(CountDown(NetworkManager.instance.lobyCountDown));

           
        }

       
    }

    IEnumerator CountDown(int time)
    {
        yield return new WaitForSeconds(1);

        time -= 1;

        GetComponent<Text>().text = "The game will start in "+time+" seconds";


        if (time>0)
        {
            StartCoroutine(CountDown(time));
        }
    }



}
