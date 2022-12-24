using LiteNetLib;
using UnityEngine;

    public class ClientPlayer:MonoBehaviour
    {
        private NetPeer Peer;
        public int Id { get; set; }
        public string Username { get; set; }

        public GameObject canvas, mainCamera;




    public ClientPlayer(NetPeer Peer, string Username, int Id)
        {
            this.Id = Id;
            this.Peer = Peer;
            this.Username = Username;
        }
        
        public NetPeer GetPeer()
        {
            return this.Peer;
        }

    }
