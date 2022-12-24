using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Deathmatch.io.Packets;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene(1);
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
