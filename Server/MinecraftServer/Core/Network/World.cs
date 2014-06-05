using MinecraftServer.Core.Network.Clients;
using MinecraftServer.Core.Packets;
using MinecraftServer.Core.Packets.PlayPackets;
using MinecraftServer.Core.States;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Network
{
    public class World
    {
        public List<Client> Clients { get; private set; }
        public bool Updated { get; set; }
		public WorldGeneration.World.World FlatWorld { get; private set; }
        public int MaxPlayer { get; private set; }

        private System.Timers.Timer timer;

        public World(int numberOfClients)
        {
            Clients = new List<Client>(numberOfClients);
			FlatWorld = WorldGeneration.WorldGenerator.GenerateWorld(WorldGeneration.WorldGenerator.WorldTypes.Flat);
            MaxPlayer = numberOfClients;

            timer = new System.Timers.Timer(50);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Update);
            timer.Enabled = true;
        }

        public void Update(object source, System.Timers.ElapsedEventArgs e)
        {
            lock (Clients)
            {
                foreach (Client c in Clients)
                {
                    c.Update();
                }
            }
        }

        public void AddClient(Client c)
        {
            lock (Clients)
            {
                foreach (Client client in Clients)
                {
                    SpawnPlayerPacket spawnPacket = StateManager.FindState("play").GetSendPacket(0x0C) as SpawnPlayerPacket;
                    spawnPacket.PlayerEntityId = client.ID;
                    spawnPacket.PlayerUUID = client.UUID;
                    spawnPacket.PlayerName = client.Name;
                    spawnPacket.PlayerX = client.PlayerPosition.X;
                    spawnPacket.PlayerY = client.PlayerPosition.FeetY;
                    spawnPacket.PlayerZ = client.PlayerPosition.Z;
                    spawnPacket.PlayerYaw = 0;
                    spawnPacket.PlayerPitch = 0;
                    spawnPacket.CurrentItem = client.PlayerMetadata.CurrentItem;
                    spawnPacket.Health = client.PlayerMetadata.Health;

                    c.AddResponse(spawnPacket);
                }

                Clients.Add(c);
                MinecraftServer.ViewModel.NewClient(c);
            }
        }

		public APacket[] GetWorld()
		{
			ChunkPacket[]	packets = new ChunkPacket[FlatWorld.Length];

			for (int i = 0 ; i < FlatWorld.Length ; ++i)
			{
				packets[i] = new ChunkPacket();
				packets[i].Ndx = i;
			}
			return packets;
		}

		public bool	UpdateBlock(int x, int y, int z, WorldGeneration.World.BlockTypes newID)
		{
			int									columnZ = (z < 0) ? z / 16 - 1 : z / 16;
			int									columnX = (x < 0) ? x / 16 - 1 : x / 16;
			WorldGeneration.World.ChunkColumn	column = null;

			for (int i = 0 ; i < FlatWorld.Length ; ++i)
			{
				if (FlatWorld[i].X == columnX && FlatWorld[i].Z == columnZ)
				{
					Server.ViewModel.Warning("World updating in chunk column : " + i + ", Coord(" + FlatWorld[i].X + ", " + FlatWorld[i].Z + ")");
					column = FlatWorld[i];
					break;
				}
			}
			if (column == null)
				return false;
			int	blockX = ((x < 0) ? 16 + x % 16 : x % 16);
			if (blockX == 16)
				blockX = 0;
			int	blockZ = ((z < 0) ? 16 + z % 16 : z % 16);
			if (blockZ == 16)
				blockZ = 0;
			Server.ViewModel.Warning("BlockCoord(" + blockX + ", " + y + ", " + blockZ + ") = " + (blockX + y + blockZ * 16) + "Ndx");
			column.UpdateBlock(blockX, y, blockZ, newID);
			return true;
		}

        public void BroadCast(APacket packet, Client except = null)
        {
            foreach (Client c in Clients)
            {
                if (except == c)
                    continue;

                c.AddResponse(packet.Clone() as APacket);
            }
        }

        // 1 = Left-click, 0 = Right-click
        public void UseEntity(int entityId, byte mouseButton)
        {
            Client c = GetClientByEntityId(entityId);

            if (mouseButton == 1 && c != null)
            {
                c.PlayerMetadata.Health -= 1.0f;
                HealthPacket healthUpdate = StateManager.FindState("play").GetSendPacket(0x06) as HealthPacket;

                healthUpdate.Health = c.PlayerMetadata.Health;
                healthUpdate.Food = c.PlayerMetadata.Food;
                healthUpdate.FoodSaturation = c.PlayerMetadata.FoodSaturation;

                c.AddBroadCast(healthUpdate, c);
            }
        }

        private Client GetClientByEntityId(int entityId)
        {
            lock (Clients)
            {
                foreach (Client c in Clients)
                {
                    if (c.ID == entityId)
                        return c;
                }
            }

            return null;
        }
    }
}
