using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RemaningTime : MonoBehaviour
{
    Text remaningText;
    bool isStarted;
    private void Start()
    {
        remaningText = GetComponent<Text>();
    }
    private void Update()
    {
        if (GameManager.instance != null&&!isStarted)
        {
            Debug.Log("1212");
            isStarted = true;
            remaningText.text = "Kalan Zaman: "+GameManager.instance.gameDuration.ToString();
            StartCoroutine(CountDown());
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.gameDuration--;
        remaningText.text = "Kalan Zaman: " + GameManager.instance.gameDuration.ToString();
        StartCoroutine(CountDown());
    }
}
