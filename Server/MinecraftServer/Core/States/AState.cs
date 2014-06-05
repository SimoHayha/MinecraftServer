using MinecraftServer.Core.Network;
using MinecraftServer.Core.Packets;
using MinecraftServer.Core.Stream;
using Server.Core.Stream;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.States
{
    public abstract class AState
    {
        private APacket[] _ReceiptPackets;
        private APacket[] _SendPackets;

        public string Name { get; private set; }

        public AState(string name)
        {
            _ReceiptPackets = new APacket[256];
            for (int i = 0; i < 256; ++i)
                _ReceiptPackets[i] = null;

            _SendPackets = new APacket[256];
            for (int i = 0; i < 256; ++i)
                _SendPackets[i] = null;

            Name = name;

            OnInitializePacket();
        }

        protected abstract void OnInitializePacket();

        public void ReadPacket(Client client, byte[] buffer, int lengthRead)
        {
            try
            {
                ByteBuffer byteBuffer = new ByteBuffer();

                byteBuffer.Write(buffer, 0, lengthRead);
                byteBuffer.Position = 0;

                while (byteBuffer.Position < lengthRead)
                {
                    int len = byteBuffer.ReadVarInt();
                    int id = byteBuffer.ReadVarInt();

                    object tmp = client.State.Peek()._ReceiptPackets[id];
                    if (tmp != null)
                    {
                        APacket packet = tmp as APacket;
                        packet = packet.Clone() as APacket;
                        if (packet == null)
                            throw new NullReferenceException("The server does not handle packet " + id);

                        packet.Read(client, byteBuffer);
                    }
                    else
                    {
                        Console.WriteLine("Unhandled packet - ID : " + id + " - Length : " + len);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR : " + e.Message);
            }
        }

        protected void AddReceiptPacket(int index, APacket packet)
        {
            _ReceiptPackets[index] = packet;
        }

        protected void AddSendPacket(int index, APacket packet)
        {
            _SendPackets[index] = packet;
        }

        public APacket GetReceiptPacket(int index)
        {
            return _ReceiptPackets[index].Clone() as APacket;
        }

        public APacket GetSendPacket(int index)
        {
            return _SendPackets[index].Clone() as APacket;
        }
    }
}
