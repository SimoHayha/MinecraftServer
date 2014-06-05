using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class PlayerDiggingPacket : APacket
    {
        public byte Status;
        public int X;
        public byte Y;
        public int Z;
        public byte Face;

        public PlayerDiggingPacket()
        {
            ID = 0x07;
            Name = "PlayerDiggingPacket";
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer buffer)
        {
            Status = (byte)buffer.ReadByte();
            X = buffer.ReadInt();
			Y = (byte)buffer.ReadByte();
			Z = buffer.ReadInt();
			Face = (byte)buffer.ReadByte();

			MinecraftServer.Core.Network.Server.ViewModel.Warning("Player Dig : Status = " + Status + ", Block(" + X + ", " + Y + ", " + Z + "), Face = " + Face);
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
			byte	newID = 0x00;

			if ((Status != 2 && client.PlayerMetadata.GameMode != 1) || client.TheWorld.UpdateBlock(X, Y, Z, (WorldGeneration.World.BlockTypes)newID) == false)
				return ;
			BlockChangePacket bcp = new BlockChangePacket();
			
			bcp.X = X;
			bcp.Y = Y;
			bcp.Z = Z;
			bcp.BlockID = newID;
			bcp.BlockMeta = 0x00;
			client.AddBroadCast(bcp, client);
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
