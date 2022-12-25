using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deathmatch.io.Packets
{
    public class GameEndPacket
    {
        public int[] RanksIds { get; set; }

        public string?[] RanksNames { get; set; }

    }
}
