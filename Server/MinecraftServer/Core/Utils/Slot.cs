using Server.Core.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Utils
{
    public class Slot
    {
        public short ID;

        public byte ItemCount;
        public short ItemDamage;
        public short NBTDataLength;

        public byte[] NBTData;

        public Slot()
        {
            ID = short.MinValue;

            ItemCount = 0;
            ItemDamage = 0;
            NBTDataLength = -1;
        }

        public void Read(ByteBuffer buffer)
        {
            ID = buffer.ReadShort();

            if (ID != -1)
            {
                ItemCount = (byte)buffer.ReadByte();
                ItemDamage = buffer.ReadShort();
                NBTDataLength = buffer.ReadShort();

                if (NBTDataLength != -1)
                {
                    NBTData = new byte[NBTDataLength];
                    buffer.Read(NBTData, 0, NBTDataLength);
                }
            }
        }
    }
}
