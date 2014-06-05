using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class PlayerAbilitiesPacket : APacket
    {
        public byte Flags;
        public float FlyingSpeed;
        public float WalkingSpeed;

        public PlayerAbilitiesPacket()
        {
            ID = 0x13;

            Flags = 0x00;
            FlyingSpeed = 0.0f;
            WalkingSpeed = 0.0f;
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
            stream.ReadByte();
            stream.ReadFloat();
            stream.ReadFloat();
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
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
