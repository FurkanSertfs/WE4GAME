using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorutineTest : MonoBehaviour
{

   public Dictionary<string, string> testDictinary = new Dictionary<string, string>();

    private void Start()
    {

        testDictinary.Add("x", "Furkan");
        testDictinary.Add("y", "Furkan2");

        foreach (string client in GetClientPlayers())
        {
            Debug.Log(client);
        }


    }
    public  Dictionary<string, string>.ValueCollection GetClientPlayers()
    {
        return testDictinary.Values;
    }

    private void Update()
    {
       
    }







   

     




}
