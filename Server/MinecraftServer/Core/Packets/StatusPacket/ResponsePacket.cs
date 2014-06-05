using Newtonsoft.Json;
using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MinecraftServer.Core.Packets.StatusPacket
{
    public class ResponsePacket : APacket
    {
        public string JSONResponse;

        public ResponsePacket()
        {
            ID = 0x00;
            Name = "ResponsePacket";
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
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

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
            buffer.WriteVarint(ID);
            buffer.WriteString("{\"version\":{\"name\":\"1.7.2\",\"protocol\":4},\"players\":{\"max\":" + client.Server._config.maxPlayers + ",\"online\":" + client.Server.GetPlayerCount() + ",\"sample\":[]},\"description\":{\"text\":\"" + client.Server._config.name + "\"},\"favicon\":\"data:image/png;base64,\"}");
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
