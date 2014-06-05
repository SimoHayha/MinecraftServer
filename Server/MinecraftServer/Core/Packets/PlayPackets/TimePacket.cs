using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class TimePacket : APacket
    {
        public TimePacket()
        {
            ID = 0x03;
            Name = "TimePacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            
        }

        protected override void OnWrite(NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
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
            
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            buffer.WriteVarint(ID);
            buffer.WriteLong(0);
            buffer.WriteLong(200);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
