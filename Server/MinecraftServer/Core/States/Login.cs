using MinecraftServer.Core.Packets.LoginPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.States
{
    public class Login : AState
    {
        public Login() : base("Login") { }

        protected override void OnInitializePacket()
        {
            AddReceiptPacket(0x00, new LoginStartPacket());
            AddSendPacket(0x02, new LoginSuccessPacket());
        }
    }
}
