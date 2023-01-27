using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deathmatch.io.Packets
{
    public class RoomStartPacket
    {
        public int Id { get; set; }
        public bool InRoom { get; set; }

        public string UserName { get; set; }

    }
}
