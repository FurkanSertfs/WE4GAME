using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deathmatch.io.Packets
{
    public class PlayerHitPacket
    {
        public int receiverId { get; set; }
        public float receivedDamage { get; set; }
    }
}