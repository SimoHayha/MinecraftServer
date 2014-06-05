using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
	class MapChunkBulkPacket : APacket
	{
		protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
		{
			
		}

		protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
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
			
		}

		protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
		{
			
		}
	}
}
