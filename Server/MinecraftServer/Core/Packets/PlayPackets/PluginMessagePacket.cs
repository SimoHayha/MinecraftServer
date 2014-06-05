using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class PluginMessagePacket : APacket
    {
        public string Channel;
        public short Length;
        public byte[] Data;

        public PluginMessagePacket()
        {
            ID = 0x17;
            Name = "PluginMessagePacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            Channel = buffer.ReadString();
            Length = buffer.ReadShort();
            Data = new byte[Length];
            buffer.Read(Data, 0, Length);
        }

        protected override void OnWrite(NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
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
