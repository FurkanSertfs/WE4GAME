using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    Transform bulletSpawnPoint;

    [SerializeField]
    FireButton fireButton;

    [SerializeField]
    float fireRate;
    float damage;
    int magazine;
    bool unlimitedMagazine;
    bool defaultGun;
    [SerializeField]
    GameObject magazineIcon;
    [SerializeField]
    Image magazineBar;
    [SerializeField]
    Text magazineCountText;
    [SerializeField]
    char infinity;

    [SerializeField]
    Gun defaultPistol;
    Gun activeGun;
    float timer;

    private void Start()
    {
        ChangeGun(defaultPistol);

    }



    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)||fireButton.isPressed)
        {
            Fire();
        }
       
    }

    public void ChangeGun(Gun newGun)
    {
        fireRate = newGun.fireRate;
        damage = newGun.damage;
        magazine = newGun.magazine;
        unlimitedMagazine = newGun.unLimited;
        defaultGun = newGun.defaultGun;

    }

    public void TakeGun(Gun newGun)
    {

    }

    IEnumerator FillMagazineBar(float fireRate)
    {
      
       

        yield return new WaitForSeconds(0.01f);
       
        magazineBar.fillAmount +=  0.01f/fireRate;
      
        magazineIcon.SetActive(true);

       
        if (magazineBar.fillAmount<1)
        {
            
            StartCoroutine(FillMagazineBar(fireRate));
         
        }

        else
        {
            if (!unlimitedMagazine)
            {
                magazine--;
                magazineCountText.text = magazine.ToString();
            }
            else
            {
                magazineCountText.text = infinity.ToString();
            }
            magazineIcon.SetActive(false);
            magazineBar.fillAmount = 0;
           
        }

    }


    public void Fire()
    {
       
        if (timer<Time.time)
        {
            

            if (unlimitedMagazine)
            {
                GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

                timer = Time.time + fireRate;

                StartCoroutine(FillMagazineBar(fireRate));
            }

            else if(magazine>0)
            {
                GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

                timer = Time.time + fireRate;

                StartCoroutine(FillMagazineBar(fireRate));
            }

            else
            {
                ChangeGun(defaultPistol);
            }

           

          

        }
       
      
        

    }
}
