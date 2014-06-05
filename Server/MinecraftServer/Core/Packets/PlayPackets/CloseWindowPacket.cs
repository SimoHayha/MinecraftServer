using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class CloseWindowPacket : APacket
    {
        public byte WindowId;

        public CloseWindowPacket()
        {
            ID = 0x0D;
            Name = "CloseWindowPacket";

            WindowId = 0;
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
            WindowId = (byte)stream.ReadByte();
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
