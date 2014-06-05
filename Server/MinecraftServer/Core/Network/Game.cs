using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Network
{
    public class Game
    {
        public Client[] Clients;
        public List<Entity> Entities;

        private System.Timers.Timer timer;
        public WorldGeneration.World.World FlatWorld { get; private set; }

        public Game(uint numberOfPlayers)
        {
            Clients = new Client[numberOfPlayers];
            Entities = new List<Entity>();

            timer = new System.Timers.Timer(50);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(WorldUpdate);
            timer.Enabled = true;
            
			FlatWorld = WorldGeneration.WorldGenerator.GenerateWorld(WorldGeneration.WorldGenerator.WorldTypes.Flat);
        }

        public void WorldUpdate(object source, System.Timers.ElapsedEventArgs ev)
        {
            lock (Clients)
            {
                foreach (Client c in Clients)
                {
                    c.Update();
                }
            }

            lock (Entities)
            {
                foreach (Entity e in Entities)
                {
                    e.Update();
                }
            }
        }
    }
}
