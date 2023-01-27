using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Milliseconds : MonoBehaviour
{
    Text milliSecond;

    float a = 5.23f;

    private void Start()
    {
        milliSecond = GetComponent<Text>();
    }

    private void Update()
    {
       
        milliSecond.text = NetworkManager.instance.milliseconds.ToString();

    }
}
