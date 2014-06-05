using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.World
{
	public abstract class World
	{
		#region Protected Attributes
		protected	List<ChunkColumn>	m_world;
		#endregion

		#region Properties
		public		int					Length
		{
			get { return m_world.Count; }
		}
		public		ChunkColumn			this[int key]
		{
			get { return m_world[key]; }
		}
		#endregion

		#region Constructors
		public World()
		{
			m_world = new List<ChunkColumn>();
		}
		#endregion

		#region Abstract Methods
		public abstract void CreateWorldFromColumn(int x, int y);
		public abstract void GenerateColumn(int x, int y, bool forceType = false, BlockTypes type = BlockTypes.Stone);
        #endregion

		#region Public Methods

		public ChunkColumn	GetChunkColumn(int x, int y)
		{
			foreach (ChunkColumn col in m_world)
			{
				if (col.X == x && col.Z == y)
					return col;
			}
			return null;
		}

		public void GenerateRandomSpawn(out int spawnX, out int spawnY, out int spawnZ, bool generateColumn = false)
		{	
			Random			rand = new Random();
			int				colX = rand.Next(0, 100);
			int				colZ = rand.Next(0, 100);
			ChunkColumn		col = GetChunkColumn(colX, colZ);
			
			spawnX = 0;
			spawnY = 0;
			spawnZ = 0;
			if (generateColumn == true)
			{
				colX = 0;
				colZ = 0;
				GenerateColumn(colX, colZ);

				GenerateColumn(colX + 1, colZ, true, BlockTypes.Dirt);
				GenerateColumn(colX, colZ + 1, true, BlockTypes.Sand);
				GenerateColumn(colX + 1, colZ + 1, true, BlockTypes.Water);
				
				GenerateColumn(colX - 1, colZ, true, BlockTypes.Grass);
				GenerateColumn(colX, colZ - 1, true, BlockTypes.Gravel);
				GenerateColumn(colX - 1, colZ - 1, true, BlockTypes.Lava);

				GenerateColumn(colX - 2, colZ, true, BlockTypes.Grass);
				GenerateColumn(colX, colZ - 2, true, BlockTypes.Gravel);
				GenerateColumn(colX - 2, colZ - 2, true, BlockTypes.Lava);
				//CreateWorldFromColumn(colX, colZ);
				col = GetChunkColumn(colX, colZ);
			}
			if (col != null)
				col.GenerateRandomSpawn(out spawnX, out spawnY, out spawnZ);
		}

		#endregion
	}
}
