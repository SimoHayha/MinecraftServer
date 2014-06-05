using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class PlayerPositionAndLookResponsePacket : APacket
    {
        public double X;
        public double Y;
        public double Z;
        public double Yaw;
        public double Pitch;
        public bool OnGround;

        public PlayerPositionAndLookResponsePacket()
        {
            ID = 0x08;
            X = 0.0d;
            Y = 0.0d;
            Z = 0.0d;
            Yaw = 0.0d;
            Pitch = 0.0d;
            OnGround = false;
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
            X = client.PlayerPosition.X;
            Y = client.PlayerPosition.FeetY;
            Z = client.PlayerPosition.Z;
            Yaw = client.PlayerPosition.Yaw;
            Pitch = client.PlayerPosition.Pitch;
            OnGround = client.PlayerPosition.OnGround;

            buffer.WriteVarint(ID);
            buffer.WriteDouble(X);
            buffer.WriteDouble(Y);
            buffer.WriteDouble(Z);
            buffer.WriteFloat((float)Yaw);
            buffer.WriteFloat((float)Pitch);
            buffer.WriteBoolean(OnGround);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            
        }
    }
}
