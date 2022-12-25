using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
  

    public GameObject hearthObje;
    public GameObject speedBoostObject;
    public GameObject armorObject;


    private void OnDestroy()
    {
        float randomChance = Random.Range(0, 100f);

        if (randomChance <= 25)
        {
            Instantiate(hearthObje, transform.position, transform.rotation);
        }else if(randomChance < 50)
        {
            Instantiate(speedBoostObject, transform.position, transform.rotation);

            
        }else if (randomChance < 100)
        {
            Instantiate(armorObject, transform.position, transform.rotation);
        }
    }




    

           


        
    


}
