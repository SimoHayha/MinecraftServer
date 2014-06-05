using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class PosLookPacket : APacket
    {
        public PosLookPacket()
        {
            ID = 0x08;
            Name = "PosLookPacket";
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
            buffer.WriteDouble(10.0d);
            buffer.WriteDouble(128.0d);
            buffer.WriteDouble(10.0d);
            buffer.WriteFloat(0.0f);
            buffer.WriteFloat(0.0f);
            buffer.WriteBoolean(true);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
