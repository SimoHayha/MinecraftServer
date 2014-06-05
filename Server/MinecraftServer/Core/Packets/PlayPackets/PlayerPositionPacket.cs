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
    public class PlayerPositionPacket : APacket
    {
        public double X;
        public double FeetY;
        public double HeadY;
        public double Z;
        public bool OnGround;

        public PlayerPositionPacket()
        {
            ID = 0x04;
            Name = "PlayerPositionPacket";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            X = buffer.ReadDouble();
            FeetY = buffer.ReadDouble();
            HeadY = buffer.ReadDouble();
            Z = buffer.ReadDouble();
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

            EntityRelativeMovePacket packet = StateManager.FindState("play").GetSendPacket(0x15) as EntityRelativeMovePacket;

            packet.DX = relX;
            packet.DY = relY;
            packet.DZ = relZ;
            packet.Id = client.ID;

            client.AddBroadCast(packet, client);
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
