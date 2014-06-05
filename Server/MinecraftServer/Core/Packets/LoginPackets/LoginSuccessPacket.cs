using MinecraftServer.Core.Stream;
using Server.Core.Stream;
using Server.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.LoginPackets
{
    public class LoginSuccessPacket : APacket
    {
        public LoginSuccessPacket()
        {
            ID = 0x02;
        }

        protected override void OnRead(ByteBuffer buffer)
        {
        }

        protected override void OnWrite(NetworkStream stream, ByteBuffer buffer)
        {
            ByteBuffer tmp = new ByteBuffer();

            tmp.WriteVarint((int)buffer.Length);
            tmp.Write(buffer.GetBuffer(), 0, (int)buffer.Length);
            stream.Write(tmp.GetBuffer(), 0, (int)tmp.Length);

            Data = tmp.GetBuffer();
            Length = (int)tmp.Length;
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
        }

        protected override void OnBeforeWrite(Network.Client client, ByteBuffer buffer)
        {
            string name = client.Name;
            string uuid = client.UUID;

            buffer.WriteVarint(ID);
            buffer.WriteString(name);
            buffer.WriteString(uuid);
        }

        protected override void OnAfterWrite(Network.Client client, ByteBuffer buffer)
        {
        }
    }
}
