using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.World
{
	public enum BlockTypes : byte
	{
		Air,
		Stone,
		Grass,
		Dirt,
		Cobblestone,
		WoodPlanks,
		Saplings,
		Bedrock,
		Water,
		StationaryWater,
		Lava,
		StationaryLava,
		Sand,
		Gravel,
		GoldOre,
		IronOre,
		CoalOre,
		Log,
		Leaves
	}

	public class Chunk
	{
		
		#region Private Attributes
		private	byte[]	m_blockTypes;
		private	byte[]	m_blockMetadatas;
		private	byte[]	m_blockLights;
		private	byte[]	m_skyLights;
		#endregion

		#region Properties
		public	byte[]	Data
		{
			get
			{
				byte[]	data = new byte[4096 + 2048 *5];
				
				Buffer.BlockCopy(m_blockTypes, 0, data, 0, m_blockTypes.Length);
				Buffer.BlockCopy(m_blockMetadatas, 0, data, 4096, m_blockMetadatas.Length);
				Buffer.BlockCopy(m_blockLights, 0, data, 4096 + 2048, m_blockLights.Length);
				Buffer.BlockCopy(m_skyLights, 0, data, 4096 + 2048 * 2, m_skyLights.Length);
				return data;
			}
		}
		public	byte[]	BlockTypes { get { return m_blockTypes; } }
		public	byte[]	BlockMetadatas { get { return m_blockMetadatas; } }
		public	byte[]	BlockLights { get { return m_blockLights; } }
		public	byte[]	SkyLights { get { return m_skyLights; } }
		#endregion

		#region Constructors
		public	Chunk(bool full = true, BlockTypes type = WorldGeneration.World.BlockTypes.Stone)
		{
			m_blockTypes = new byte[4096];
			m_blockMetadatas = new byte[2048];
			m_blockLights = new byte[2048];
			m_skyLights = new byte[2048];

			for (int i = 0 ; i < 4096 ; ++i)
				m_blockTypes[i] = ((full == true) ? (byte)type : (byte)0x00);
			for (int i = 0 ; i < 2048 ; ++i)
			{
				m_blockLights[i] = (byte)0xff;
				m_skyLights[i] = (byte)0xff;
			}
		}
		#endregion

		#region Public Methods
		public void GenerateRandomSpawn(out int spawnX, out int spawnY, out int spawnZ)
		{
			Random	rand = new Random();
			spawnX = rand.Next(0, 15);
			spawnZ = rand.Next(0, 15);
			spawnY = 0;
			for (int y = 0 ; y < 16 ; ++y)
			{
				if (m_blockTypes[spawnX + (spawnZ + y * 16) * 16] != 0x00 && m_blockTypes[spawnX + (spawnZ + (y + 1) * 16) * 16] == 0x00)
				{
					spawnY = y + 1;
					break;
				}
			}
		}

		public void UpdateBlock(int x, int y, int z, BlockTypes newID)
		{
			m_blockTypes[x + (z + y * 16) * 16] = (byte)newID;
		}
		#endregion
	}
}
