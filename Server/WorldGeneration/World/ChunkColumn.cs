using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.World
{
	public class ChunkColumn
	{
		#region Private Attributes
		private	int		m_x;
		private	int		m_z;
		private	byte[]	m_biome;
		private	int		m_topChunkNdx;
		private	Chunk[]	m_column;
		#endregion

		#region Properties
		public	int		X { get { return m_x; } }
		public	int		Z { get { return m_z; } }
		public		byte[]				ChunkData
		{
			get
			{
				byte[]	data;
				int		Width = 16;
				int		Height = 256;
				int		NumBlocks = Width * Height * Width;
				int		BiomeDataSize = Width * Width;
				int		MetadataOffset = sizeof(byte) * NumBlocks;
				int		BlockLightOffset = MetadataOffset + sizeof(byte) * (NumBlocks / 2);
				int		SkyLightOffset = BlockLightOffset + sizeof(byte) * (NumBlocks / 2);
				int		BiomeOffset = SkyLightOffset + sizeof(byte) * (NumBlocks / 2);
				int		DataSize = BiomeOffset + BiomeDataSize;

				data = new byte[DataSize];
				for (int i = 0 ; i < 16 ; ++i)
					Buffer.BlockCopy(m_column[i].BlockTypes, 0, data, i * m_column[i].BlockTypes.Length, m_column[i].BlockTypes.Length);
				for (int i = 0 ; i < 16 ; ++i)
					Buffer.BlockCopy(m_column[i].BlockMetadatas, 0, data, MetadataOffset + i * m_column[i].BlockMetadatas.Length, m_column[i].BlockMetadatas.Length);
				for (int i = 0 ; i < 16 ; ++i)
					Buffer.BlockCopy(m_column[i].BlockLights, 0, data, BlockLightOffset + i * m_column[i].BlockLights.Length, m_column[i].BlockLights.Length);
				for (int i = 0 ; i < 16 ; ++i)
					Buffer.BlockCopy(m_column[i].SkyLights, 0, data, SkyLightOffset + i * m_column[i].SkyLights.Length, m_column[i].SkyLights.Length);
				Buffer.BlockCopy(m_biome, 0, data, BiomeOffset, m_biome.Length);

				return data;
			}
		}
		public	int		Length
		{
			get { return 16; }
		}
		public	Chunk	this[int key]
		{
			get { return m_column[key]; }
		}
		#endregion

		#region Constructors
		public ChunkColumn(int x, int y)
		{
            m_x = x;
            m_z = y;
			m_biome = new byte[256];
			m_column = new Chunk[16];
		}
		#endregion

		#region Public Methods
		public void FlatColumn(bool forceType = false, BlockTypes type = BlockTypes.Stone, int fillHeight = 4)
		{
			System.Diagnostics.Debug.Assert(fillHeight < 16);
			m_topChunkNdx = fillHeight;
			for (int i = 0 ; i < 16 ; ++i)
			{
				if (forceType == true)
					m_column[i] = new Chunk(((i < fillHeight) ? true : false), type);
				else
					m_column[i] = new Chunk(((i < fillHeight) ? true : false));
			}
				
		}

		public void GenerateRandomSpawn(out int spawnX, out int spawnY, out int spawnZ)
		{
			m_column[m_topChunkNdx].GenerateRandomSpawn(out spawnX, out spawnY, out spawnZ);
			spawnX += m_x * 16;
			spawnZ += m_z * 16;
			spawnY += m_topChunkNdx * 16;
		}

		public void UpdateBlock(int x, int y, int z, WorldGeneration.World.BlockTypes newID)
		{
			int	chunkNdx = y / 16;

			m_column[chunkNdx].UpdateBlock(x, y % 16, z, newID);
		}
		#endregion
	}
}
