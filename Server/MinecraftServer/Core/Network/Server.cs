using MinecraftServer.Core.ConfigFile;
using MinecraftServer.Core.Packets;
using MinecraftServer.Core.States;
using MinecraftServer.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Network
{
    public class Server
    {
        private Thread _server;
        private World _world;
        public static ViewModel ViewModel;
        private StateManager _stateManager;
        public Config _config;
        public System.Collections.Concurrent.BlockingCollection<APacket> Packets;

        public Server(object DataContext)
        {
            ViewModel = DataContext as ViewModel;

            _server = new Thread(StartListening);

            _config = new Config();
            _config.init();

            _world = new World(_config.maxPlayers);
            _world.Updated = true;

            _stateManager = new StateManager();
            _stateManager.Init();

            Packets = new System.Collections.Concurrent.BlockingCollection<APacket>();
            new PacketWriterWorker(Packets, 1u);

            _server.Start();
            ViewModel.Log("Server is now listening on " + _config.port + ".");
        }

        public void StartListening()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 25565);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Socket handler = listener.Accept();
                    ThreadPool.QueueUserWorkItem(HandleClientComm, handler);
                    System.Diagnostics.Debug.WriteLine("Connexion in " + handler.RemoteEndPoint);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        private void HandleClientComm(object socket)
        {
            Socket mySocket = (Socket)socket;
            Client client = new Client(mySocket, this);

            while (mySocket.Connected && client.Alive)
                client.OnCommHandle();

            client.Kill();
            mySocket.Shutdown(SocketShutdown.Both);
            mySocket.Close();
            System.Diagnostics.Debug.WriteLine("Connexion out");
        }

        public World GetWorld()
        {
            return _world;
        }

        public int GetPlayerCount()
        {
            return _world.Clients.Count;
        }

        public int GetMaxPlayer()
        {
            return _world.MaxPlayer;
        }
    }
}
