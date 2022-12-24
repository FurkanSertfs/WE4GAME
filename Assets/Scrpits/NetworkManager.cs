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

public class NetworkManager : MonoBehaviour
    {
        //public fields
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

        public string playerName;

        

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
            Debug.Log("Connecting..");
            netManager = new NetManager(netListener);
            netManager.Start();
            netManager.Connect(serverIP, port, connectionString);

            netListener.NetworkErrorEvent += NetListener_NetworkErrorEvent;

            netListener.PeerDisconnectedEvent += NetListener_PeerDisconnectedEvent;

            netListener.PeerConnectedEvent += (server) =>
            {
                
                this.server = server;
                Debug.Log($"Network Manager: Connected to server.");
               
            };

            netListener.NetworkReceiveEvent += (server, reader, deliveryMethod) =>
            {
                netPacketProcessor.ReadAllPackets(reader, server);
                Debug.Log("Network Manager: Packet arrived.");
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
            }

        });

        netPacketProcessor.SubscribeReusable<GameStartPacket>((packet) =>
        {
            GameManager.instance.gameStarted = true;
            GameManager.instance.gameDuration = packet.GameDuration;
            
            SceneManager.LoadScene(1);


        });

        netPacketProcessor.SubscribeReusable<NewUserPacket>((packet) =>
        {

            StartCoroutine(Wait(packet));
          


          
         

        });

        netPacketProcessor.SubscribeReusable<ServerMovementPacket>((packet) =>
        {

            GameManager.instance.clients[packet.Id].gameObject.transform.position 
            = Vector3.Lerp(GameManager.instance.clients[packet.Id].gameObject.transform.position,new Vector3(packet.PositionX, packet.PositionY, packet.PositionZ),100);

            GameManager.instance.clients[packet.Id].gameObject.transform.eulerAngles = new Vector3(0,0,packet.Rotation);


        });




    }


    IEnumerator Wait(NewUserPacket packet)
    {
        yield return new WaitForSeconds(0.1f);


        if (SceneManager.GetActiveScene().buildIndex==1)
        {
            GameObject newPlayer = Instantiate(ClientPrefab, Vector3.zero, Quaternion.identity);

            newPlayer.GetComponent<ClientPlayer>().Id = packet.Id;

            newPlayer.GetComponent<ClientPlayer>().Username = packet.Username;


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
          
            Debug.Log("Disconnected");
            Destroy(gameObject);
        }

        private void NetListener_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log("Peer disconnected, reason: " + disconnectInfo.Reason);
        }

        private void NetListener_NetworkErrorEvent(System.Net.IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
            Debug.Log(socketError);
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
                netManager.PollEvents();
        }
    }
