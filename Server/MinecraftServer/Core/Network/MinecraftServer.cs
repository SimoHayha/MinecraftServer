using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections;
using MinecraftServer.Core.Stream;
using MinecraftServer.Core.States;
using System.Windows;
using MinecraftServer.GUI;
using System;
using MinecraftServer.Core.ConfigFile;
using MinecraftServer.Core.Packets;
using MinecraftServer.Core.Packets.HandshakePackets;

namespace MinecraftServer.Core.Network
{
	public class MinecraftServer
	{
		#region Private Attributes definition
		private	TcpListener						_tcpListener;
		private	Thread							_listenThread;
        private ArrayList						_clients;
        private StateManager					_stateManager;
        public Config							_config;

        private bool _alive;

        public static ViewModel ViewModel;
		#endregion

		#region Properties
        public World DefaultWorld { get; private set; }

        // Proto
        public System.Collections.Concurrent.BlockingCollection<APacket> Packets;
		#endregion

		#region Constructors
		public MinecraftServer(object DataContext)
		{
            ViewModel = DataContext as ViewModel;

            _config = new Config();
            _config.init();

            _stateManager = new StateManager();
            _stateManager.Init();
            _clients = new ArrayList();

            this._tcpListener = new TcpListener(IPAddress.Any, _config.port);
            Start();

            Packets = new System.Collections.Concurrent.BlockingCollection<APacket>();
            new PacketWriterWorker(Packets, 1u);

            //DefaultWorld = new World();
            DefaultWorld.Updated = true;
        }
		#endregion

		#region private Methods

        private void ListenForClients()
		{
            try
            {
                while (_alive)
                {
                    TcpClient client = this._tcpListener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(HandleClientComm, client);
                }
            }
            catch (SocketException e)
            {
                // normal exception when stopping server
            }
		}

        public void Start()
        {
            if (_alive)
            {
                ViewModel.Warning("Server already running");
                return;
            }

            _alive = true;
            this._tcpListener.Start();
            this._listenThread = new Thread(new ThreadStart(ListenForClients));
            this._listenThread.Start();

            ViewModel.Log("Server online");
        }

        public void Stop()
        {
            if (!_alive)
            {
                ViewModel.Warning("Server already stopped");
                return;
            }

            this._tcpListener.Stop();
            _alive = false;

            ViewModel.Log(DefaultWorld.Clients.Count + " player(s) kicked");
            foreach (Client c in DefaultWorld.Clients)
                c.Kill(false);

            DefaultWorld.Clients.Clear();

            ViewModel.Log("Server offline");
        }

		private void HandleClientComm(object client)
		{
            //TcpClient tcpClient = (TcpClient)client;
            //Client c = new Client(tcpClient.GetStream(), DefaultWorld, this);

            //IPEndPoint ip = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            //Console.WriteLine("New client on " + ip.Address.ToString());

            //while (tcpClient.Connected && c.Alive)
            //    c.OnCommHandle();

            //c.Kill();
            //tcpClient.Close();
		}
		#endregion

        public uint GetPlayerCount()
        {
            return (uint)DefaultWorld.Clients.Count;
        }

        public void OnAdminCommand(string command)
        {
            if (command.ToLower() == "stop")
            {
                if (DefaultWorld.Clients.Count > 0)
                {
                    ViewModel.Warning(DefaultWorld.Clients.Count + " player(s) are currently connected, use 'forcestop' if you really want to stop");
                    return;
                }
                Stop();
            }
            else if (command.ToLower() == "forcestop")
            {
                Stop();
            }
            else if (command.ToLower() == "start")
            {
                Start();
            }
        }
    }
}
