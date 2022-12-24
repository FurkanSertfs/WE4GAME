using LiteNetLib;


    public class ClientPlayer
    {
        private NetPeer Peer;
        public int Id { get; }
        public string Username { get; }


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
