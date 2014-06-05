using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class ClientSettingPacket : APacket
    {
        public string Locale;
        public byte ViewDistance;
        public byte ChatFlag;
        public bool ChatColours;
        public byte Difficulty;
        public bool ShowCape;

        public ClientSettingPacket()
        {
            ID = 0x15;
            Name = "ClientSettingsPacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            Locale = buffer.ReadString();
            ViewDistance = (byte)buffer.ReadByte();
            ChatFlag = (byte)buffer.ReadByte();
            ChatColours = buffer.ReadBoolean();
            Difficulty = (byte)buffer.ReadByte();
            ShowCape = buffer.ReadBoolean();
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
