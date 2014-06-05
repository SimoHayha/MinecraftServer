using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class EntityRelativeMovePacket : APacket
    {
        public int Id;
        public double DX;
        public double DY;
        public double DZ;

        public EntityRelativeMovePacket()
        {
            ID = 0x15;
            Id = 0;
            DX = 0.0d;
            DY = 0.0d;
            DZ = 0.0d;
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
            buffer.WriteInt(Id);
            //buffer.WriteByte(Convert.ToByte(DX));
            //buffer.WriteByte(Convert.ToByte(DY));
            //buffer.WriteByte(Convert.ToByte(DZ));
            //buffer.WriteByte(0);
            //buffer.WriteByte(0);
            buffer.WriteFixedPointByte(DX);
            buffer.WriteFixedPointByte(DY);
            buffer.WriteFixedPointByte(DZ);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
