using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deathmatch.io.Packets
{
    public class NewBulletPacket
    {
        public int OwnerId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float Rotation { get; set; }
    }
}
