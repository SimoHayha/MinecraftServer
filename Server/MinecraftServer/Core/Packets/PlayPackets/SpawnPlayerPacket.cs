using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class SpawnPlayerPacket : APacket
    {
        public int PlayerEntityId;
        public string PlayerUUID;
        public string PlayerName;
        public double PlayerX;
        public double PlayerY;
        public double PlayerZ;
        public byte PlayerYaw;
        public byte PlayerPitch;
        public short CurrentItem;
        public float Health;

        public SpawnPlayerPacket()
        {
            ID = 0x0C;
            Name = "SpawnPlayerPacket";

            PlayerEntityId = 0;
            PlayerUUID = "";
            PlayerName = "";
            PlayerX = 0.0d;
            PlayerY = 0.0d;
            PlayerZ = 0.0d;
            PlayerYaw = 0;
            PlayerPitch = 0;
            CurrentItem = 0;
            Health = 0.0f;
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
            
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
            
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            buffer.WriteVarint(ID);
            buffer.WriteVarint(PlayerEntityId);
            buffer.WriteString(PlayerUUID);
            buffer.WriteString(PlayerName);
            buffer.WriteFixedPoint(PlayerX);
            buffer.WriteFixedPoint(PlayerY);
            buffer.WriteFixedPoint(PlayerZ);
            buffer.WriteByte(PlayerYaw);
            buffer.WriteByte(PlayerPitch);
            buffer.WriteShort(CurrentItem);
            buffer.WriteByte((3 << 5) | 6);
            buffer.WriteFloat(Health);
            buffer.WriteByte(0x7F);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
