using MinecraftServer.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class ClientStatusPacket : APacket
    {
        public byte ActionID;

        public ClientStatusPacket()
        {
            ID = 0x16;
            Name = "ClientStatusPacket";

            ActionID = 0xFF;
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
            ActionID = (byte)stream.ReadByte();
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
            if (ActionID == 0x00)
            {
                client.AddResponse(StateManager.FindState("play").GetSendPacket(0x07));
                // Respawn
            }
            else if (ActionID == 0x01)
            {
                // Stats
            }
            else if (ActionID == 0x02)
            {
                // Open inventory achievement
            }
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
