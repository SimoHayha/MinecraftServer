using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
	class PlayerBlockPlacement : APacket
	{

		private	int		_x;
		private	byte	_y;
		private	int		_z;
		private	byte	_direction;
		private	short	_blockID;
		private	byte	_itemCount;
		private	short	_itemDamage;
		private	short	_NBTLength;


        public PlayerBlockPlacement()
        {
            ID = 0x08;
            Name = "PlayerBlockPlacement";
        }

		protected override void OnRead(Server.Core.Stream.ByteBuffer buffer)
		{
			_x = buffer.ReadInt();
			_y = (byte)buffer.ReadByte();
			_z = buffer.ReadInt();
			_direction = (byte)buffer.ReadByte();

			if (_direction == 0)
				_y--;
			else if (_direction == 1)
				_y++;
			else if (_direction == 2)
				_z--;
			else if (_direction == 3)
				_z++;
			else if (_direction == 4)
				_x--;
			else if (_direction == 5)
				_x++;

			_blockID = buffer.ReadShort();
			if (_blockID < 0)
				return ;
					MinecraftServer.Core.Network.Server.ViewModel.Warning("Block Placement Coord(" + _x + ", " + _y + ", " + _z + ")");
			_itemCount = (byte)buffer.ReadByte();
			_itemDamage = buffer.ReadShort();
			_NBTLength = buffer.ReadShort();
		}

		protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
		{
		}

		protected override void OnBeforeRead(Network.Client client)
		{
		}

		protected override void OnAfterRead(Network.Client client)
		{
			if (_blockID < 0 || client.TheWorld.UpdateBlock(_x, _y, _z, (WorldGeneration.World.BlockTypes)_blockID) == false)
				return ;
			BlockChangePacket bcp = new BlockChangePacket();
			
			bcp.X = _x;
			bcp.Y = _y;
			bcp.Z = _z;
			bcp.BlockID = (byte)_blockID;
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
