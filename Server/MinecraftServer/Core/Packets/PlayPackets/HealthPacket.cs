using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class HealthPacket : APacket
    {
        public float Health;
        public short Food;
        public float FoodSaturation;

        public HealthPacket()
        {
            ID = 0x06;
            Name = "HealthPacket";

            Health = 10.0f;
            Food = 10;
            FoodSaturation = 2.5f;
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
            buffer.WriteFloat(Health);
            buffer.WriteShort(Food);
            buffer.WriteFloat(FoodSaturation);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
