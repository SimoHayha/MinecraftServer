using MinecraftServer.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.StatusPacket
{
    public class RequestPacket : APacket
    {
        public RequestPacket()
        {
            ID = 0x00;
            Name = "RequestPacket";
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
            client.AddResponse(StateManager.FindState("status").GetSendPacket(0x00));
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
