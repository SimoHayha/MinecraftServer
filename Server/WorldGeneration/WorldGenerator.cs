using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration
{
    public static class WorldGenerator
    {
		public enum WorldTypes
		{
			Flat
		}


        public static World.World GenerateWorld(WorldTypes type)
        {
			return new World.Worlds.FlatWorld();
        }
	}
}
