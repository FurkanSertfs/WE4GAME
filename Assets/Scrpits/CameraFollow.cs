using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float speed;
    [SerializeField]
    bool offset;
    [SerializeField]
    Vector3 offsetVector;

    private void Start()
    {
        
       
    }


    private void LateUpdate()
    {
        if (offset)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z)+offsetVector, speed);

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), speed);

        }
    }
}
