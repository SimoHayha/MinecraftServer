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
    public class PlayerLookPacket : APacket
    {
        public double Yaw;
        public double Pitch;
        public bool OnGround;

        public PlayerLookPacket()
        {
            ID = 0x05;
            Name = "PlayerLookPacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            Yaw = buffer.ReadFloat();
            Pitch = buffer.ReadFloat();
            OnGround = buffer.ReadBoolean();
        }

        protected override void OnWrite(NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }

        protected override void OnBeforeRead(Network.Client client)
        {
            
        }

        protected override void OnAfterRead(Network.Client client)
        {
            client.PlayerPosition.Yaw = Yaw;
            client.PlayerPosition.Pitch = Pitch;

            EntityLookPacket packet = StateManager.FindState("play").GetSendPacket(0x16) as EntityLookPacket;

            packet.Id = client.ID;
            packet.Yaw = client.PlayerPosition.Yaw;
            packet.Pitch = client.PlayerPosition.Pitch;
            client.AddBroadCast(packet, client);

            EntityHeadLookPacket headPacket = StateManager.FindState("play").GetSendPacket(0x19) as EntityHeadLookPacket;
            headPacket.Id = client.ID;
            headPacket.HeadYaw = Yaw;
            client.AddBroadCast(headPacket, client);
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
