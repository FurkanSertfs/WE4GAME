using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArea : MonoBehaviour
{
    public static RedArea instance;

    public Zone[] zones;

    private void Awake()
    {
        instance = this;
    }

  


}
