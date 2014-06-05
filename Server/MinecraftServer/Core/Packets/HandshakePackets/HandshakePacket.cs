using MinecraftServer.Core.Stream;
using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.HandshakePackets
{
    public class HandshakePacket : APacket
    {
        public int ProtocolVersion;
        public string ServerAdress;
        public ushort Port;
        public int NextState;

        protected override void OnRead(ByteBuffer buffer)
        {
            ProtocolVersion = buffer.ReadVarInt();
            ServerAdress = buffer.ReadString();
            Port = (ushort)buffer.ReadShort();
            NextState = buffer.ReadVarInt();
        }

        protected override void OnWrite(NetworkStream stream, ByteBuffer buffer)
        {
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
            client.SetTargetMode(NextState);
        }

        protected override void OnBeforeWrite(Network.Client client, ByteBuffer buffer)
        {
        }

        protected override void OnAfterWrite(Network.Client client, ByteBuffer buffer)
        {
        }
    }
}
