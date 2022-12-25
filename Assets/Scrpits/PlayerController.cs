using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Deathmatch.io.Packets;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    public Transform[] bulletSpawnPoint;
   
    public List<Collectable> collectables = new List<Collectable>();

    public enum CollectibleObject { Wood, Rock, Armor, Hearth, SpeedBooster}



    

    [SerializeField]
    FireButton fireButton;
    [SerializeField]
    DashButton dashgButton;
    [SerializeField]
    ChangeWeaponButton changeWeaponButton;

    float zoneTimer;

    public float zoneDamage;

    [SerializeField]
    float fireRate;

    [SerializeField]
    public  float damage;
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
    GameObject muzzleFlash;

    

    

    
    public EnvanterGun[] envanterGuns;
    Gun activeGun;
    float timer;

    [SerializeField]
    public GameObject tailRenderer;
    [SerializeField]
    float dashCooldown;
    float dashTimer;
    [SerializeField]
    Image dashButton;

    [SerializeField]
    Rigidbody2D rigidbody2D;


    [SerializeField]
    public float health;
    public float maxhealth;

    [SerializeField]
    public Image healthBar;

    [SerializeField]
    GameObject changeWeaponButtonObject;


    private void Start()
    {
        ChangeGun(0);
        

    }



    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)||fireButton.isPressed)
        {
            Fire();
        }

        if (dashTimer < Time.time)
        {

          
          

            if (Input.GetKeyDown(KeyCode.A) || dashgButton.isPressed)
            {
                dashButton.fillAmount = 1;
                dashTimer = Time.time + dashCooldown;
                StartCoroutine(Dash());

            }
            else
            {
                dashButton.fillAmount = 0;
            }

           
        }

        else
        {
            dashButton.fillAmount -= Time.deltaTime / dashCooldown;
        }


       
       
    }
  public  IEnumerator DashAnim()
    {
        tailRenderer.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        tailRenderer.SetActive(false);

    }

    IEnumerator Dash()
    {

        if (NetworkManager.instance.server != null)
        {
            NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new ClientMovementPacket
            {
                PositionX = transform.position.x,
                PositionY = transform.position.y,
                PositionZ = transform.position.z,
                Rotation = transform.eulerAngles.z,
                IsDash = true
                

            }, LiteNetLib.DeliveryMethod.ReliableOrdered);
        }


        rigidbody2D.velocity = (transform.up * 15);
        tailRenderer.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        rigidbody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
        tailRenderer.SetActive(false);

    }

    public void ChangeGun(int index)
    {

        for (int i = 0; i < envanterGuns.Length; i++)
        {
            envanterGuns[i].gameObject.SetActive(false);
        }
        EnvanterGun newGun = envanterGuns[index];
        newGun.gameObject.SetActive(true);

        fireRate = newGun.gun.fireRate;
        damage = newGun.gun.damage;
        magazine = newGun.gun.magazine;
        unlimitedMagazine = newGun.gun.unLimited;
        defaultGun = newGun.gun.defaultGun;
        bulletPrefab = newGun.bulletPrefab;
        bulletSpawnPoint = newGun.bullletPoints;
        muzzleFlash = newGun.muzzleFlash;

        if (GetComponentInParent<ClientPlayer>().isLocal)
        {
            if (NetworkManager.instance.server != null)
            {


                NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new WeaponChangePacket
                {
                    OwnerId = GetComponentInParent<ClientPlayer>().Id,
                    ActiveWeaponIdx = index



                }, LiteNetLib.DeliveryMethod.ReliableOrdered);
            }
        }

      


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
                for (int i = 0; i < bulletSpawnPoint.Length; i++)
                {
                    GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint[i].position, bulletSpawnPoint[i].rotation);
                    newBullet.GetComponent<Bullet>().id = GetComponentInParent<ClientPlayer>().Id;
                    newBullet.GetComponent<Bullet>().damage = damage;

                    Debug.Log("Damage Kontrol " + newBullet.GetComponent<Bullet>().damage);

                    timer = Time.time + fireRate;

                    StartCoroutine(FillMagazineBar(fireRate));

                    muzzleFlash.SetActive(true);

                    if (NetworkManager.instance.server != null)
                    {


                        NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new NewBulletPacket
                        {
                            PositionX = bulletSpawnPoint[i].position.x,
                            PositionY = bulletSpawnPoint[i].position.y,
                            Rotation = bulletSpawnPoint[i].transform.eulerAngles.z



                        }, LiteNetLib.DeliveryMethod.ReliableOrdered);
                    }

                }


               
            }

            else if(magazine>0)
            {
                for (int i = 0; i < bulletSpawnPoint.Length; i++)
                {
                    GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint[i].position, bulletSpawnPoint[i].rotation);
                    newBullet.GetComponent<Bullet>().id = GetComponentInParent<ClientPlayer>().Id;
                    newBullet.GetComponent<Bullet>().damage = damage;

                    timer = Time.time + fireRate;

                    StartCoroutine(FillMagazineBar(fireRate));

                    muzzleFlash.SetActive(true);

                    if (NetworkManager.instance.server != null)
                    {


                        NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new NewBulletPacket
                        {
                            PositionX = bulletSpawnPoint[i].position.x,
                            PositionY = bulletSpawnPoint[i].position.y,
                            Rotation = bulletSpawnPoint[i].transform.eulerAngles.z


                        }, LiteNetLib.DeliveryMethod.ReliableOrdered);
                    }

                }
            }

            else
            {
                ChangeGun(0);
            }

           

          

        }
       
      
        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<CollecTableGun>()!=null)
        {
            changeWeaponButtonObject.SetActive(true);
            if (changeWeaponButton.isPressed)
            {
                ChangeGun(collision.GetComponent<CollecTableGun>().index);
                Destroy(collision.gameObject);
            }
        }

        if (collision.GetComponent<Zone>()!=null)
        {
             

            if (zoneTimer<Time.time)
            {
                TakeHit(zoneDamage);
                zoneTimer = Time.time + 0.2f;

                if (NetworkManager.instance.server != null)
                {
                    NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new PlayerHitPacket
                    {
                        receiverId = collision.GetComponentInParent<ClientPlayer>().Id,
                        receivedDamage = zoneDamage,
                        isProb = false
                    }, LiteNetLib.DeliveryMethod.ReliableOrdered);


                }
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CollecTableGun>() != null)
        {
            changeWeaponButtonObject.SetActive(false);
        }
    }

    public void TakeHit(float damage)
    {
        health -= damage;

        healthBar.fillAmount = health / maxhealth;

        if (health<=0)
        {
            gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class Collectable
{
    public PlayerController.CollectibleObject collectableObjects;

    public int count;




}
