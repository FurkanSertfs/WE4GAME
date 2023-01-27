using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

  

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < GameManager.instance.clients.Count; i++)
            {
                Debug.Log(GameManager.instance.clients[i].GetComponent<ClientPlayer>().Username);
            }

            Debug.Log("Clints Count "+GameManager.instance.clients.Count);
        }
    }
}
