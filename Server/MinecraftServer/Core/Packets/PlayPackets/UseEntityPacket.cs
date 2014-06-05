using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class UseEntityPacket : APacket
    {
        public int Target;
        public byte Mouse;

        public UseEntityPacket()
        {
            ID = 0x02;
            Name = "UseEntityPacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            Target = buffer.ReadInt();
            Mouse = (byte)buffer.ReadByte();
        }

        protected override void OnWrite(NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }

        protected override void OnBeforeRead(Network.Client client)
        {
            
        }

        protected override void OnAfterRead(Network.Client client)
        {
            client.UseEntity(client.ID, Mouse);
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
