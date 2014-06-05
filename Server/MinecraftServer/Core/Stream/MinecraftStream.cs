using System;
using System.IO;
using System.Text;

namespace MinecraftServer.Core.Stream
{
    public partial class MinecraftStream : System.IO.Stream
    {
        public MinecraftStream(System.IO.Stream baseStream)
        {
            BaseStream = baseStream;
        }

        public System.IO.Stream BaseStream { get; set; }

        public override bool CanRead
        {
            get
            {
                return BaseStream.CanRead; 
            }
        }

        public override bool CanSeek
        {
            get 
            { 
                return BaseStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            { 
                return BaseStream.CanWrite;
            }
        }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override long Length
        {
            get
            { 
                return BaseStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return BaseStream.Position;
            }
            set
            {
                BaseStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        public byte ReadUInt8()
        {
            int value = BaseStream.ReadByte();
            if (value == -1)
                throw new EndOfStreamException();
            return (byte)value;
        }

        public void WriteUInt8(byte value)
        {
            WriteByte(value);
        }

        public sbyte ReadInt8()
        {
            return (sbyte)ReadUInt8();
        }

        public void WriteInt8(sbyte value)
        {
            WriteUInt8((byte)value);
        }

        public ushort ReadUInt16()
        {
            return (ushort)((ReadUInt8() << 8) | ReadUInt8());
        }

        public void WriteUInt16(ushort value)
        {
            Write(new[]
            {
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            }, 0, 2);
        }

        public short ReadInt16()
        {
            return (short)ReadInt16();
        }

        public bool ReadBoolean()
        {
            return ReadUInt8() != 0;
        }

        public void WriteBoolean(bool value)
        {
            if (value)
                WriteUInt8(1);
            else
                WriteUInt8(0);
        }

        public void WriteArray(byte[] data)
        {
            if (data.Length > 1024)
                throw new ArgumentOutOfRangeException("data", "Array must be less than or equal to 1024 byte in length");
            var difference = 1024 - data.Length;
            Write(data, 0, data.Length);
            if (difference > 0)
                Write(new byte[difference], 0, difference);
        }

        public byte[] ReadArray()
        {
            var data = new byte[1024];
            Read(data, 0, data.Length);
            return data;
        }

        public string ReadString()
        {
            var raw = new byte[64];
            Read(raw, 0, 64);
            return Encoding.ASCII.GetString(raw).TrimEnd(' ');
        }

        public string ReadString(int size)
        {
            var raw = new byte[size];
            Read(raw, 0, size);
            return Encoding.ASCII.GetString(raw).TrimEnd(' ');
        }

        public void WriteString(string value)
        {
            if (value.Length > 64)
                throw new ArgumentOutOfRangeException("value", "String must be less than or equal to 64 characters in length.");
            value = value.PadRight(64, ' ');
            var raw = UTF8Encoding.ASCII.GetBytes(value);
            Write(raw, 0, raw.Length);
        }

        public void WriteVarint(int value)
        {
            byte[] bytes = new byte[5];
            int idx = 0;

            do
            {
                bytes[idx] = (byte)((value & 0x7f) | ((value > 0x7f) ? 0x80 : 0x00));
                value = value >> 7;
                idx++;
            } while (value > 0);
            Write(bytes, 0, idx);
        }

        public int ReadVarInt()
        {
            int result = 0;
            int shift = 0;
            byte b = 0;

            do
            {
                b = (byte)base.ReadByte();
                if (b == 255)
                    throw new Exception("Not Allowed to read");
                result = result | ((b & 0x7f) << shift);
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }

        public int ReadVarInt(byte theByte)
        {
            int result = 0;
            int shift = 0;
            byte b = 0;
            bool first = true;

            do
            {
                if (first)
                {
                    b = theByte;
                    first = false;
                }
                else
                    b = (byte)base.ReadByte();
                if (b == 255)
                    throw new Exception("Not Allowed to read");
                result = result | ((b & 0x7f) << shift);
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }

        public double ReadDouble()
        {
            byte[] d = new byte[sizeof(double)];
            Read(d, 0, sizeof(double));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(d);
            return BitConverter.ToDouble(d, 0);
        }

        public int ReadInt()
        {
            byte[] i = new byte[sizeof(int)];
            Read(i, 0, sizeof(int));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(i);
            return BitConverter.ToInt32(i, 0);
        }

        public float ReadFloat()
        {
            byte[] buf = new byte[sizeof(float)];
            Read(buf, 0, sizeof(float));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buf);
            return BitConverter.ToSingle(buf, 0);
        }

        public short ReadShort()
        {
            byte[] buf = new byte[sizeof(short)];
            Read(buf, 0, sizeof(short));
            return System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buf, 0));
        }
    }
}
