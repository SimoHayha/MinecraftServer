using MinecraftServer.Core.States;
using Server.Core.Stream;
using Server.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class JoinGamePacket : APacket
    {
        public JoinGamePacket()
        {
            ID = 0x01;
            Name = "JoinGamePacket";
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
			client.PlayerMetadata.GameMode = 1;

            buffer.WriteVarint(ID);
            buffer.WriteInt(4);
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(1);
            buffer.WriteByte(32);
            buffer.WriteString("default");
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            SpawnPlayerPacket spawnPacket = StateManager.FindState("play").GetSendPacket(0x0C) as SpawnPlayerPacket;
            spawnPacket.PlayerEntityId = client.ID;
            spawnPacket.PlayerUUID = client.UUID;
            spawnPacket.PlayerName = client.Name;
            spawnPacket.PlayerX = client.PlayerPosition.X;
            spawnPacket.PlayerY = client.PlayerPosition.FeetY;
            spawnPacket.PlayerZ = client.PlayerPosition.Z;
            spawnPacket.PlayerYaw = 0;
            spawnPacket.PlayerPitch = 0;
            spawnPacket.CurrentItem = client.PlayerMetadata.CurrentItem;
            spawnPacket.Health = client.PlayerMetadata.Health;
            //packet.Metadata = metadata; To add

            client.AddBroadCast(spawnPacket, client);
        }
    }
}
