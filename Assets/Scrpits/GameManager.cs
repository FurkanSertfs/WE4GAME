using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int gameDuration;

    public bool gameStarted;

    [SerializeField]
    GameObject playerPrefab;

    bool isSpawned;

    public Dictionary<int, ClientPlayer> clients = new Dictionary<int, ClientPlayer>();

    public int id;
    public string userName;
    



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

           
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 &&!isSpawned)
        {
            isSpawned = true;
            
            GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            newPlayer.GetComponent<ClientPlayer>().Id = id;
            newPlayer.GetComponent<ClientPlayer>().Username = userName;
            newPlayer.GetComponent<ClientPlayer>().canvas.SetActive(true);
            newPlayer.GetComponent<ClientPlayer>().mainCamera.SetActive(true);
            newPlayer.GetComponent<ClientPlayer>().GetComponentInChildren<JoystickPlayerExample>().enabled = true;
            newPlayer.GetComponent<ClientPlayer>().GetComponentInChildren<PlayerController>().enabled = true;


            clients.Add(newPlayer.GetComponent<ClientPlayer>().Id, newPlayer.GetComponent<ClientPlayer>());

        }
    }
    
}
