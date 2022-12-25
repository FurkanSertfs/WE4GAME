using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbManager : MonoBehaviour
{
    [SerializeField]
    public Probs[] probs;

    public static ProbManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        probs=FindObjectsOfType<Probs>();

        for (int i = 0; i < probs.Length; i++)
        {
            probs[i].GetComponent<Probs>().id = i;
        }
    }

   
}
