using System;
using MinecraftServer.Core.Stream;
using System.Collections;
using Server.Core.Utils;

namespace MinecraftServer.Core.Packets
{
    public static class PacketReader
    {
//        public delegate IPacket CreatePacketInstance();

//        private static readonly CreatePacketInstance[] Packets = new CreatePacketInstance[]
//        {
//            () => new HandshakePacket()
//        };

//        public static IPacket ReadPacket(MinecraftStream stream)
//        {
//            byte len = stream.ReadUInt8();
//            byte id = stream.ReadUInt8();
//            Console.WriteLine("Message len " + len + " id " + id);
//            if (Packets[id] == null)
//                throw new InvalidOperationException("Invalid packet ID: 0x" + id.ToString("X2"));
//            var packet = Packets[id]();
//            packet.ReadPacket(stream);
//            return packet;
//        }

        public static int ReadVarInt(MinecraftStream stream)
        {
            ArrayList bytes = new ArrayList();
            int i;

            do
            {
                i = stream.ReadByte();
                bytes.Add((byte)i);
            } while ((i >> 7) == 1);

            byte[] array = (byte[])bytes.ToArray(typeof(byte));

            if (array.Length == 1)
                return VariantBitConverter.ToByte(array);
            else if (array.Length == 2)
                return VariantBitConverter.ToInt16(array);
            else if (array.Length == 3)
                return VariantBitConverter.ToInt32(array);
            else
                return 0;
        }
    }
}
