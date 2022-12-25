using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Deathmatch.io.Packets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using DG.Tweening;


public class NetworkManager : MonoBehaviour
    {
    //public fields
    private static Stopwatch stopwatch = new Stopwatch();

        public static NetworkManager instance;
        public bool isLocalPlayer;
        [HideInInspector] public ClientPlayer localPlayer;
        [HideInInspector] public int localPlayerId;

        public NetPeer server;
        public NetPacketProcessor netPacketProcessor;
        public Dictionary<int, ClientPlayer> clients;
        //private fields
        private EventBasedNetListener netListener;
        private NetManager netManager;
        [Header("Connection")]
        [SerializeField] private string serverIP;
        [SerializeField] private int port;
        [SerializeField] private string connectionString;
        [Header("Agora")]
       
        [Header("Prefab")]
        [SerializeField] private GameObject ClientPrefab;

        [SerializeField]
        InputField inputField;

        [SerializeField]
        float lerpSpeed;

        public string playerName;

        float lastPing;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                clients = new Dictionary<int, ClientPlayer>();
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    

   


    private void Start()
    {
        Connect();

        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
    
    

    public void Connect()
        {
            netListener = new EventBasedNetListener();
            netPacketProcessor = new NetPacketProcessor();
            UnityEngine.Debug.Log("Connecting..");
            netManager = new NetManager(netListener);
            netManager.Start();
            netManager.Connect(serverIP, port, connectionString);

            netListener.NetworkErrorEvent += NetListener_NetworkErrorEvent;

            netListener.PeerDisconnectedEvent += NetListener_PeerDisconnectedEvent;

            netListener.PeerConnectedEvent += (server) =>
            {
                
                this.server = server;
                UnityEngine.Debug.Log($"Network Manager: Connected to server.");
               
            };

            netListener.NetworkReceiveEvent += (server, reader, deliveryMethod) =>
            {
                netPacketProcessor.ReadAllPackets(reader, server);
                UnityEngine.Debug.Log("Network Manager: Packet arrived.");
            };


            netPacketProcessor.SubscribeReusable<ServerConnectionPacket>((packet) =>
            {
                localPlayerId = packet.Id;
               
            });

        netPacketProcessor.SubscribeReusable<ClientJoinResponsePacket>((packet) =>
        {
            if (packet.Success)
            {
                GameManager.instance.id = packet.Id;
                GameManager.instance.userName = packet.Username;
                GameManager.instance.isLocal = true;
            }

        });

        netPacketProcessor.SubscribeReusable<GameStartPacket>((packet) =>
        {
            GameManager.instance.gameStarted = true;
            GameManager.instance.gameDuration = packet.GameDuration;
            
            SceneManager.LoadScene(1);


        });

        netPacketProcessor.SubscribeReusable<RedZonePacket>((packet) =>
        {
            for (int i = 0; i < RedArea.instance.zones.Length; i++)
            {
                RedArea.instance.zones[i].isActive = packet.isMoving;
            }

            GameManager.instance.clients[localPlayerId].GetComponentInChildren<PlayerController>().zoneDamage = packet.Damage;

        });


        netPacketProcessor.SubscribeReusable<PlayerHitPacket>((packet) =>
        {

            if (packet.isProb)
            {
                if (ProbManager.instance.probs[packet.receiverId].GetComponent<Probs>()!=null)
                {
                    ProbManager.instance.probs[packet.receiverId].GetComponent<Probs>().TakeHit(packet.receivedDamage);
                }
              
            }
            else
            {
                GameManager.instance.clients[packet.receiverId].GetComponentInChildren<PlayerController>().TakeHit(packet.receivedDamage);

            }

        });

        netPacketProcessor.SubscribeReusable<NewUserPacket>((packet) =>
        {
            
            StartCoroutine(Wait(packet));


          
        });

        netPacketProcessor.SubscribeReusable<NewBulletPacket>((packet) =>
        {

            PlayerController playerController = GameManager.instance.clients[packet.OwnerId].GetComponentInChildren<PlayerController>();

            for (int i = 0; i < playerController.bulletSpawnPoint.Length; i++)
            {
                GameObject newBullet = Instantiate(playerController.bulletPrefab, playerController.bulletSpawnPoint[i].position, playerController.bulletSpawnPoint[i].rotation);

                newBullet.GetComponent<Bullet>().damage = GameManager.instance.clients[packet.OwnerId].GetComponentInChildren<PlayerController>().damage;


                newBullet.GetComponent<Bullet>().id = localPlayerId;

            }

            



        });

        netPacketProcessor.SubscribeReusable<WeaponChangePacket>((packet) =>
        {
            PlayerController playerController = GameManager.instance.clients[packet.OwnerId].GetComponentInChildren<PlayerController>();

            playerController.ChangeGun(packet.ActiveWeaponIdx);





        });


        netPacketProcessor.SubscribeReusable<ServerMovementPacket>((packet) =>
        {
            if (GameManager.instance.clients.ContainsKey(packet.Id))
            {
                if (packet.IsDash)
                {
                    GameManager.instance.clients[packet.Id].GetComponentInChildren<PlayerController>().StartCoroutine(GameManager.instance.clients[packet.Id].GetComponentInChildren<PlayerController>().DashAnim());
                }


                //  GameManager.instance.clients[packet.Id].GetComponentInChildren<PlayerController>().gameObject.transform.position  = new Vector3(packet.PositionX, packet.PositionY, packet.PositionZ);

                GameManager.instance.clients[packet.Id].GetComponentInChildren<PlayerController>().gameObject.transform.DOMove(new Vector3(packet.PositionX, packet.PositionY, packet.PositionZ),lerpSpeed);


                //      GameManager.instance.clients[packet.Id].GetComponentInChildren<PlayerController>().gameObject.transform.position
                //= Vector3.Lerp(GameManager.instance.clients[packet.Id].GetComponentInChildren<PlayerController>().gameObject.transform.position, new Vector3(packet.PositionX, packet.PositionY, packet.PositionZ), lerpSpeed);

                GameManager.instance.clients[packet.Id].GetComponentInChildren<PlayerController>().gameObject.transform.DORotate(new Vector3(0, 0, packet.Rotation),lerpSpeed);

            }


        });

        netPacketProcessor.SubscribeReusable<PingPongPacket>((packet) =>
        {

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            stopwatch.Restart();

        });


    }


    IEnumerator Wait(NewUserPacket packet)
    {
        yield return new WaitForSeconds(0.3f);


        if (SceneManager.GetActiveScene().buildIndex==1)
        {
            GameObject newPlayer = Instantiate(ClientPrefab, Vector3.zero, Quaternion.identity);

            PlayerController playerController = newPlayer.GetComponentInChildren<PlayerController>();

            newPlayer.GetComponent<ClientPlayer>().Id = packet.Id;

            newPlayer.GetComponent<ClientPlayer>().Username = packet.Username;

            playerController.health = packet.Health;

            playerController.healthBar.fillAmount = playerController.health / playerController.maxhealth;

            playerController.ChangeGun(packet.ActiveWeaponIdx);

            playerController.nameText.text = packet.Username;


            GameManager.instance.clients.Add(packet.Id, newPlayer.GetComponent<ClientPlayer>());
        }
        else
        {
            StartCoroutine(Wait(packet));
        }

       


    }


    public void Disconnect()
        {
            netManager.Stop();
          
            UnityEngine.Debug.Log("Disconnected");
            Destroy(gameObject);
        }

        private void NetListener_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            UnityEngine.Debug.Log("Peer disconnected, reason: " + disconnectInfo.Reason);
        }

        private void NetListener_NetworkErrorEvent(System.Net.IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
            UnityEngine.Debug.Log(socketError);
        }

        void OnApplicationQuit()
        {
            if (netManager != null)
                netManager.Stop();
        }

        void OnDestroy()
        {
            if (netManager != null)
                netManager.Stop();
        }

        private void Update()
        {
            if (netManager != null)
            {
                netManager.PollEvents();
            }

            //if (NetworkManager.instance.server != null)
            //{
            //NetworkManager.instance.netPacketProcessor.Send(NetworkManager.instance.server, new PingPongPacket
            //{
            //    Id = localPlayerId

            //}, LiteNetLib.DeliveryMethod.ReliableOrdered);
            //stopwatch.Start();
            //}
        }
}
