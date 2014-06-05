using MinecraftServer.Core.Network;
using MinecraftServer.Core.Stream;
using Server.Core.Stream;
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;

namespace MinecraftServer.Core.Packets
{
    public abstract class APacket : ICloneable
    {
        public int ID;
        public int Length;
        public byte[] Data;
        public string Name;
        public Client Destination;

        public APacket()
        {
            ID = 0xFF;
            Length = 0;
            Data = null;
            Name = "Abstract Packet";
        }

        public void Read(Client client, ByteBuffer buffer)
        {
            try
            {
                OnBeforeRead(client);
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception e)
            {
                MinecraftServer.Core.Network.Server.ViewModel.Error("OnBeforeRead : " + e.Message);
            }

            try
            {
                OnRead(buffer);
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception e)
            {
                MinecraftServer.Core.Network.Server.ViewModel.Error("OnRead : " + e.Message);
            }

            try
            {
                OnAfterRead(client);
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception e)
            {
                MinecraftServer.Core.Network.Server.ViewModel.Error("OnAfterRead : " + e.Message);
            }
        }

        public void Write(NetworkStream stream, Client client)
        {
            ByteBuffer Buffer = new ByteBuffer();

            try
            {
                OnBeforeWrite(client, Buffer);
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception e)
            {
                MinecraftServer.Core.Network.Server.ViewModel.Error("OnBeforeWrite : " + e.Message);
            }

            try
            {
                OnWrite(stream, Buffer);
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception e)
            {
                MinecraftServer.Core.Network.Server.ViewModel.Error("OnWrite : " + e.Message);
            }

            try
            {
                OnAfterWrite(client, Buffer);
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception e)
            {
                MinecraftServer.Core.Network.Server.ViewModel.Error("OnAfterWrite : " + e.Message);
            }

            MinecraftServer.Core.Network.Server.ViewModel.Log(Name);

            stream.Flush();
            Buffer.Dispose();
        }

        protected abstract void OnRead(ByteBuffer stream);
        protected abstract void OnWrite(NetworkStream stream, ByteBuffer buffer);

        protected abstract void OnBeforeRead(Client client);
        protected abstract void OnAfterRead(Client client);
        protected abstract void OnBeforeWrite(Client client, ByteBuffer buffer);
        protected abstract void OnAfterWrite(Client client, ByteBuffer buffer);

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
