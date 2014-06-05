using ICSharpCode.SharpZipLib.Zip.Compression;
using MinecraftServer.Core.States;
using Server.Core.Stream;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class ChunkPacket : APacket
    {
		public int	Ndx;
        public bool GroundUpContinuous;
        public ushort PrimaryBitmap;
        public ushort SecondaryBitmap;

        public ChunkPacket()
        {
            ID = 0x21;
            Name = "ChunkPacket";

            GroundUpContinuous = true;
            PrimaryBitmap = 0xffff;
            SecondaryBitmap = 0x00;
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
			buffer.WriteVarint(ID);
			buffer.WriteInt(client.TheWorld.FlatWorld[Ndx].X);
			buffer.WriteInt(client.TheWorld.FlatWorld[Ndx].Z);
			buffer.WriteBoolean(GroundUpContinuous);
			buffer.WriteLEUShort(PrimaryBitmap);
			buffer.WriteShort((short)SecondaryBitmap);
			
			byte[]	chunkData = client.TheWorld.FlatWorld[Ndx].ChunkData;
			byte[]	compressData = new byte[chunkData.Length];

			ConcurrentStack<Deflater> DeflaterPool = new ConcurrentStack<Deflater>();
			Deflater deflater;
			DeflaterPool.TryPop(out deflater);
			if (deflater == null)
			    deflater = new Deflater(5);
			deflater.SetInput(chunkData, 0, chunkData.Length);
			deflater.Finish();
			int length = deflater.Deflate(compressData);
			deflater.Reset();
			DeflaterPool.Push(deflater);
			
			buffer.WriteInt(length);
			buffer.Write(compressData, 0, length);
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
