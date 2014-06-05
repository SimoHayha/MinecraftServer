using MinecraftServer.Core.States;
using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.StatusPacket
{
    public class PingPacket : APacket
    {
        public long Time;

        public PingPacket()
        {
            ID = 0x01;
            Name = "PingPacket";
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
            Time = stream.ReadLong();
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
            ByteBuffer tmp = new ByteBuffer();

            tmp.WriteVarint((int)buffer.Length);
            tmp.Write(buffer.GetBuffer(), 0, (int)buffer.Length);
            stream.Write(tmp.GetBuffer(), 0, (int)tmp.Length);

            Data = tmp.GetBuffer();
            Length = (int)tmp.Length;
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
            PingPacket packet = StateManager.FindState("status").GetSendPacket(0x01) as PingPacket;

            packet.Time = Time;

            client.AddResponse(packet);
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            buffer.WriteVarint(ID);
            buffer.WriteLong(Time);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
