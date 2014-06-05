using MinecraftServer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Packets.PlayPackets
{
    public class CreativeInventoryActionPacket : APacket
    {
        public short Slot;
        public Slot ClickedItem;

        public CreativeInventoryActionPacket()
        {
            ID = 0x10;
            Name = "CreativeInventoryAction";

            Slot = 0;
            ClickedItem = new Slot();
        }

        protected override void OnRead(Server.Core.Stream.ByteBuffer stream)
        {
            Slot = stream.ReadShort();
            ClickedItem.Read(stream);
        }

        protected override void OnWrite(System.Net.Sockets.NetworkStream stream, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnBeforeRead(Network.Client client)
        {
        }

        protected override void OnAfterRead(Network.Client client)
        {
            ClickedItem = new Slot();
        }

        protected override void OnBeforeWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }

        protected override void OnAfterWrite(Network.Client client, Server.Core.Stream.ByteBuffer buffer)
        {
        }
    }
}
