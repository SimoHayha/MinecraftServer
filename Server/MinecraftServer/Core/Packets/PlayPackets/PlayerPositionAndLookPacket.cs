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
    public class PlayerPositionAndLookPacket : APacket
    {
        public double X;
        public double FeetY;
        public double HeadY;
        public double Z;
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public PlayerPositionAndLookPacket()
        {
            ID = 0x06;
            Name = "PlayerPositionAndLookPacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            X = buffer.ReadDouble();
            FeetY = buffer.ReadDouble();
            HeadY = buffer.ReadDouble();
            Z = buffer.ReadDouble();
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
            double relX = X - client.PlayerPosition.X;
            double relY = FeetY - client.PlayerPosition.FeetY;
            double relZ = Z - client.PlayerPosition.Z;

            client.PlayerPosition.X += relX;
            client.PlayerPosition.FeetY += relY;
            client.PlayerPosition.Z += relZ;
            client.PlayerPosition.Yaw = Yaw;
            client.PlayerPosition.Pitch = Pitch;
            client.PlayerPosition.OnGround = OnGround;

            EntityLookAndRelativeMovePacket packet = StateManager.FindState("play").GetSendPacket(0x17) as EntityLookAndRelativeMovePacket;
            packet.DX = relX;
            packet.DY = relY;
            packet.DZ = relZ;
            packet.Yaw = Yaw;
            packet.Pitch = Pitch;
            packet.Id = client.ID;
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
