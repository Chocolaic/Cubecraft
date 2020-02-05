using System;
using System.IO;
using System.Text;

namespace Cubecraft.Net.Protocol.IO
{
    public class InputBuffer : IDisposable
    {
        private Stream s;
        public InputBuffer(Stream s)
        {
            this.s = s;
        }
        public byte[] ReadData(int len)
        {
            byte[] result = new byte[len];
            ReadData(result, 0, len);
            return result;
        }
        public int ReadData(byte[] data, int offset, int len)
        {
            int read = 0;
            while (read < len)
            {
                read += s.Read(data, offset + read, len - read);
            }
            return read;
        }
        public int ReadByte()
        {
            int b = s.ReadByte();
            if (b < 0)
                throw new EndOfStreamException();
            return b;
        }
        public int ReadVarInt()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (true)
            {
                k = ReadByte();
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }
        public int ReadVarShort()
        {
            ushort low = ReadUShort();
            byte high = 0;
            if ((low & 0x8000) != 0)
            {
                low &= 0x7FFF;
                high = (byte)ReadByte();
            }
            return ((high & 0xFF) << 15) | low;
        }
        public string ReadString()
        {
            int length = ReadVarInt();
            if (length > 0)
            {
                return Encoding.UTF8.GetString(ReadData(length));
            }
            else return "";
        }
        public bool ReadBool()
        {
            return ReadByte() != 0x00;
        }
        public double ReadDouble()
        {
            byte[] rawValue = ReadData(8);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToDouble(rawValue, 0);
        }
        public float ReadFloat()
        {
            byte[] rawValue = ReadData(4);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToSingle(rawValue, 0);
        }
        public short ReadShort()
        {
            byte[] rawValue = ReadData(2);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt16(rawValue, 0);
        }
        public int ReadInt()
        {
            byte[] rawValue = ReadData(4);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt32(rawValue, 0);
        }
        public long ReadLong()
        {
            byte[] rawValue = ReadData(8);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt64(rawValue, 0);
        }
        public ushort ReadUShort()
        {
            byte[] rawValue = ReadData(2);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToUInt16(rawValue, 0);
        }
        public ulong ReadULong( )
        {
            byte[] rawValue = ReadData(8);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToUInt64(rawValue, 0);
        }
        public ulong[] ReadULongArray()
        {
            int len = ReadVarInt();
            ulong[] result = new ulong[len];
            for (int i = 0; i < len; i++)
                result[i] = ReadULong();
            return result;
        }
        public Stream GetStream()
        {
            return this.s;
        }
        public void Dispose()
        {
            s.Dispose();
        }
    }
}
