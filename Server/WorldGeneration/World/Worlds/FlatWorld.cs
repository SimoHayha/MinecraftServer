using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.World.Worlds
{
	class FlatWorld : World
	{
		#region Override Methods
		public override void GenerateColumn(int x, int y, bool forceType = false, BlockTypes type = BlockTypes.Cobblestone)
		{
			if (GetChunkColumn(x, y) != null)
				return ;
            int ndx = m_world.FindLastIndex(o => o.X <= x);
            ChunkColumn newColumn = new ChunkColumn(x, y);

            newColumn.FlatColumn(forceType, type);
			if (ndx > 0)
				m_world.Insert(ndx, newColumn);
			else
				m_world.Add(newColumn);
		}

		public override void CreateWorldFromColumn(int x, int y)
		{
			for (int i = 0 ; i < 7 ; ++i)
            {
                for (int j = 0 ; j < 7 ; j++)
                {
					if (GetChunkColumn(i - 3 + x, j - 3 + y) != null)
						continue ;
					m_world.Add(new ChunkColumn(i - 3 + x, j - 3 + y));
                    m_world[m_world.Count - 1].FlatColumn();
                }
            }
		}
		#endregion
	}
}
