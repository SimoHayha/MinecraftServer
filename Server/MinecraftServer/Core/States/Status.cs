using MinecraftServer.Core.Packets.StatusPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.States
{
    public class Status : AState
    {
        public Status() : base("Status") { }

        protected override void OnInitializePacket()
        {
            AddReceiptPacket(0x00, new RequestPacket());
            AddReceiptPacket(0x01, new PingPacket());

            AddSendPacket(0x00, new ResponsePacket());
            AddSendPacket(0x01, new PingPacket());
        }
    }
}
