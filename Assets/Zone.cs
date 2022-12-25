using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{

   

    public bool isActive;

    public Vector3 position;

    public float speed;

    
    void Update()
    {
        if (isActive)
        {
            transform.position += position*Time.deltaTime*speed; 
        }
        
    }
}
