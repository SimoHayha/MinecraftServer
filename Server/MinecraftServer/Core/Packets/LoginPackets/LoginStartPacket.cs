using MinecraftServer.Core.Packets.PlayPackets;
using MinecraftServer.Core.States;
using MinecraftServer.Core.Stream;
using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.LoginPackets
{
    public class LoginStartPacket : APacket
    {
        public string Name;

        public LoginStartPacket()
        {
            ID = 0x00;
            Name = "";
        }

        protected override void OnRead(ByteBuffer buffer)
        {
            Name = buffer.ReadString();
        }

        protected override void OnWrite(NetworkStream stream, ByteBuffer buffer)
        {
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
            client.OnLogged(Name, System.Guid.NewGuid());

            client.AddResponse(StateManager.FindState("login").GetSendPacket(0x02));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x01));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x05));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x39));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x03));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x30));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x06));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x1F));
            client.AddResponse(StateManager.FindState("play").GetSendPacket(0x08));

			APacket[]	chunkPackets = client.TheWorld.GetWorld();
			foreach (APacket packet in chunkPackets)
				client.AddResponse(packet, false);

            client.SendPendingPackets();

            client.SetTargetMode(0);
        }

        protected override void OnBeforeWrite(Network.Client client, ByteBuffer buffer)
        {
        }

        protected override void OnAfterWrite(Network.Client client, ByteBuffer buffer)
        {
        }
    }
}
