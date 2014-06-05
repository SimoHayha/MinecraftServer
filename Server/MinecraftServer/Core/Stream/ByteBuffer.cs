using Server.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Stream
{
    public class ByteBuffer : MemoryStream
    {
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
            base.Write(bytes, 0, idx);
        }

        public void WriteArray(byte[] data)
        {
            if (data.Length > 1024)
                throw new ArgumentOutOfRangeException("data", "Array must be less than or equal to 1024 byte in length");
            var difference = 1024 - data.Length;
            base.Write(data, 0, data.Length);
            if (difference > 0)
                Write(new byte[difference], 0, difference);
        }

        public void WriteString(string value)
        {
            var raw = Encoding.UTF8.GetBytes(value);
            WriteVarint(raw.Length);
            base.Write(raw, 0, raw.Length);
        }

        public int ReadVarInt()
        {
            int shift = 0;
            int result = 0;
            const int sizeBites = 32;
            int byteValue;
            int i = 0;
            while (i < base.Length)
            {
                byteValue = base.ReadByte();
                int tmp = byteValue & 0x7f;
                result |= tmp << shift;
                if (shift > sizeBites)
                {
                    throw new System.ArgumentOutOfRangeException("bytes", "Byte array is too large.");
                }
                if ((byteValue & 0x80) != 0x80)
                {
                    return result;
                }
                shift += 7;
                i++;
            }
            throw new System.ArgumentException("Cannot decode varint from byte array.", "bytes");
        }

        public void WriteUTF8String(string value)
        {
            WriteVarint(value.Length);

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(value));
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(value));
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(value));
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteUShort(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(value));
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteLEUShort(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            base.Write(bytes, 0, bytes.Length);
        }

        public void WriteBoolean(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            base.Write(bytes, 0, bytes.Length);
        }

        public string ReadString()
        {
            int len;

            len = ReadVarInt();
            byte[] data = new byte[len];
            Read(data, 0, len);

            return Encoding.UTF8.GetString(data);
        }

        public short ReadShort()
        {
            byte[] buf = new byte[sizeof(short)];
            Read(buf, 0, sizeof(short));
            return System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buf, 0));
        }

        public int ReadInt()
        {
            byte[] i = new byte[sizeof(int)];
            Read(i, 0, sizeof(int));
            return System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(i, 0));
        }

        public double ReadDouble()
        {
            byte[] d = new byte[sizeof(double)];
            Read(d, 0, sizeof(double));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(d);
            return BitConverter.ToDouble(d, 0);
        }

        public float ReadFloat()
        {
            byte[] f = new byte[sizeof(float)];
            Read(f, 0, sizeof(float));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(f);
            return BitConverter.ToSingle(f, 0);
        }

        public bool ReadBoolean()
        {
            byte[] b = new byte[sizeof(bool)];
            Read(b, 0, sizeof(bool));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return BitConverter.ToBoolean(b, 0);
        }

        public void WriteFixedPoint(double value)
        {
            int abs_int = (int)(value * 32);
            WriteInt(abs_int);
        }

        public void WriteFixedPointByte(double value)
        {
            int abs_int = (int)(value * 32);
            byte toSend = (byte)(abs_int);
            WriteByte(toSend);
        }

        public void WriteByteAngle(double angle)
        {
            WriteByte((byte)(255.0d * angle / 360.0d));
        }

        public long ReadLong()
        {
            byte[] l = new byte[sizeof(long)];
            Read(l, 0, sizeof(long));
            return System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt64(l, 0));
        }
    }
}
