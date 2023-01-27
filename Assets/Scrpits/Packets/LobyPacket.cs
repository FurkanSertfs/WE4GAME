using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deathmatch.io.Packets
{
    public class LobyPacket
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string? Username { get; set; }
        public int TotalZonePulse { get; set; }

    }
}