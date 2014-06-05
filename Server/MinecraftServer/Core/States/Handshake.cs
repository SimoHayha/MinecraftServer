using MinecraftServer.Core.Packets.HandshakePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.States
{
    public class Handshake : AState
    {
        public Handshake() : base("Handshake") { }

        protected override void OnInitializePacket()
        {
            AddReceiptPacket(0x00, new HandshakePacket());
        }
    }
}
