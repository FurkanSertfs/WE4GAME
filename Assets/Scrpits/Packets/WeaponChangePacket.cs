using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deathmatch.io.Packets
{
    public class WeaponChangePacket
    {
        public int OwnerId { get; set; }

        public int ActiveWeaponIdx { get; set; }

    }
}
