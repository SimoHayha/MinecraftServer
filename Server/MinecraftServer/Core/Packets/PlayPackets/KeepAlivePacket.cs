using MinecraftServer.Core.States;
using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class KeepAlivePacket : APacket
    {
        public int KeepAlive;

        public KeepAlivePacket()
        {
            ID = 0x00;
            KeepAlive = 0;
            Name = "KeepAlivePacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            KeepAlive = buffer.ReadInt();
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
            //if (KeepAlive != client.LastKeepAlive)
            //{
            //    Console.WriteLine("Last KeepAlive is different than current KeepAlive");
            //    Console.WriteLine(KeepAlive + " " + client.LastKeepAlive);
            //}

        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            Random r = new Random();
            KeepAlive = r.Next(int.MinValue, int.MaxValue);

            buffer.WriteVarint(ID);
            buffer.WriteInt(KeepAlive);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            client.LastKeepAlive = KeepAlive;
        }
    }
}
