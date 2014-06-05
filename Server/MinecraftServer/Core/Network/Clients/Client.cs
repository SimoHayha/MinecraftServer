using MinecraftServer.Core.ConfigFile;
using MinecraftServer.Core.Network.Clients;
using MinecraftServer.Core.Packets;
using MinecraftServer.Core.Packets.PlayPackets;
using MinecraftServer.Core.States;
using MinecraftServer.Core.Stream;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace MinecraftServer.Core.Network
{
    public class Client
    {
        public struct Position
        {
            public double X;
            public double Z;
            public double FeetY;
            public double HeadY;
            public double Yaw;
            public double Pitch;
            public bool OnGround;
        }

        public struct Metadata
        {
			public int GameMode;
            public float Health;
            public short Food;
            public float FoodSaturation;
            public short CurrentItem;
        }

        public Position PlayerPosition;
        public Metadata PlayerMetadata;

        private List<APacket> _packetQueue;
        private Stopwatch _stopwatch;

        public Socket Socket { get; private set; }
        public MemoryStream Buffer { get; set; }
        public string Name { get; private set; }
        public string UUID { get; private set; }
        public int ID { get; private set; }
        public bool Alive { get; set; }
        public int IndexInBuffer { get; set; }
        public int LastKeepAlive { get; set; }
        public int Target { get; set; }
        public Stack<AState> State;
        public World TheWorld;
        public Server Server;

        private static int GLOBAL_ID = 0;

        public Client(Socket socket, Server server)
        {
            State = new Stack<AState>();
            _packetQueue = new List<APacket>();
            TheWorld = server.GetWorld();

            Socket = socket;
            Buffer = new MemoryStream();
            Alive = true;
            IndexInBuffer = 0;
            LastKeepAlive = 0;
            Target = -1;

            Server = server;
			
			int spawnX = 0, spawnY = 0, spawnZ = 0;

			TheWorld.FlatWorld.GenerateRandomSpawn(out spawnX, out spawnY, out spawnZ, true);

            PlayerPosition.X = spawnX;
            PlayerPosition.Z = spawnZ;
            PlayerPosition.FeetY = spawnY;
            PlayerPosition.HeadY = 0.0d;
            PlayerPosition.Yaw = 0.0f;
            PlayerPosition.Pitch = 0.0f;
            PlayerPosition.OnGround = false;

            PlayerMetadata.Health = 10.0f;
            PlayerMetadata.Food = 10;
            PlayerMetadata.FoodSaturation = 2.5f;
            PlayerMetadata.CurrentItem = 0;

            State.Push(StateManager.FindState("handshake"));
        }

        public void Update()
        {
            if (Alive == false)
                return;
            if (_stopwatch.Elapsed.Seconds > 5)
            {
                lock (_packetQueue)
                    AddResponse(StateManager.FindState("play").GetSendPacket(0x00));
                _stopwatch.Reset();
                _stopwatch.Start();
            }
        }

        public void OnCommHandle()
        {
            if (State.Count <= 0)
                throw new EmptyStateException("Client doesn't have any state");
            AState state = State.Peek();

            try
            {
                byte[] buffer = new byte[1024];
                int len = Socket.Receive(buffer);

                if (len == 0)
                {
                    Kill();
                    return;
                }

                state.ReadPacket(this, buffer, len);
            }
            catch (Exception e)
            {
                Alive = false;
            }
        }

        public void SetTargetMode(int target)
        {
            if (target == 1)
            {
                State.Push(StateManager.FindState("status"));
            }
            else if (target == 2)
            {
                State.Push(StateManager.FindState("login"));
            }
            else if (target == 0)
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
                State.Push(StateManager.FindState("play"));
                TheWorld.AddClient(this);
            }

            Target = target;
        }

        public void OnLogged(string name, System.Guid uuid)
        {
            Name = name;
            UUID = uuid.ToString().Substring(0, 16);
            ID = GLOBAL_ID++;
        }

        public void AddResponse(APacket packet, bool immediate = true)
        {
            packet.Destination = this;
            PacketWriterWorker worker = PacketWriterWorker.Instance;

            if (immediate == true)
                worker.Packets.Add(packet);
            else
                _packetQueue.Add(packet);
        }

        public void SendPendingPackets()
        {
            foreach (APacket p in _packetQueue)
                PacketWriterWorker.Instance.Packets.Add(p);

            _packetQueue.Clear();
        }

        public void AddBroadCast(APacket packet, Client except = null)
        {
            TheWorld.BroadCast(packet, except);
        }

        public void Kill(bool remove = true)
        {
            Alive = false;
            if (remove)
                TheWorld.Clients.Remove(this);

            if (ID >= 0)
            {
                DestroyEntitiesPacket packet = StateManager.FindState("play").GetSendPacket(0x13) as DestroyEntitiesPacket;

                packet.Entities.Add(ID);

                AddBroadCast(packet, this);

                Server.ViewModel.RemoveClient(this);
            }
        }

        public void UseEntity(int entityId, byte mouseButton)
        {
            TheWorld.UseEntity(entityId, mouseButton);
        }
    }
}
