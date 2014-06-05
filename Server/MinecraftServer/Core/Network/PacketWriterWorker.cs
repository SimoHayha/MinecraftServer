using MinecraftServer.Core.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Network
{
    public class PacketWriterWorker
    {
        public static PacketWriterWorker Instance;

        public System.Collections.Concurrent.BlockingCollection<APacket> Packets;
        public Mutex MyMutex;

        public PacketWriterWorker(System.Collections.Concurrent.BlockingCollection<APacket> packets, uint numberOfInstance)
        {
            Packets = packets;
            Instance = this;
            MyMutex = new Mutex();

            for (uint i = 0u; i < numberOfInstance; ++i)
            {
                Task.Factory.StartNew(() =>
                    {
                        uint myId = i;

                        foreach (var packet in packets.GetConsumingEnumerable())
                        {
                            NetworkStream wrapper = new NetworkStream(packet.Destination.Socket);
                            lock (packet.Destination.Socket)
                            {
                                System.Diagnostics.Debug.WriteLine("SendingTo " + packet.Destination.Socket.RemoteEndPoint + " " + packet.Name);
                                packet.Write(wrapper, packet.Destination);
                            }
                        }
                    }, TaskCreationOptions.LongRunning);
            }
        }
    }
}
