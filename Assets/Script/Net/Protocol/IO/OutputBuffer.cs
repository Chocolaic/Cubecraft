using System;
using System.IO;
using System.Text;

namespace Cubecraft.Net.Protocol.IO
{
    class OutputBuffer : IDisposable
    {
        private Stream s;
        public OutputBuffer(Stream s)
        {
            this.s = s;
        }
        public OutputBuffer() : this(new MemoryStream()) { }
        public void WriteData(byte[] data)
        {
            s.Write(data, 0, data.Length );
        }
        public void WriteData(byte[] data, int offset, int len)
        {
            s.Write(data, offset, len);
        }
        public void WriteByte(byte b)
        {
            s.WriteByte(b);
        }
        public void WriteVarInt(int paramInt)
        {
            while ((paramInt & -128) != 0)
            {
                WriteByte((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            WriteByte((byte)paramInt);
        }
        public void WriteString(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            WriteVarInt(bytes.Length);
            WriteData(bytes);
        }
        public void WriteDouble(double number)
        {
            byte[] theDouble = BitConverter.GetBytes(number);
            Array.Reverse(theDouble); //Endianness
            WriteData(theDouble);
        }
        public void WriteFloat(float number)
        {
            byte[] theFloat = BitConverter.GetBytes(number);
            Array.Reverse(theFloat); //Endianness
            WriteData(theFloat);
        }
        public void WriteShort(short number)
        {
            byte[] theShort = BitConverter.GetBytes(number);
            Array.Reverse(theShort); //Endianness
            WriteData(theShort);
        }
        public void WriteLong(long number)
        {
            byte[] theLong = BitConverter.GetBytes(number);
            Array.Reverse(theLong);
            WriteData(theLong);
        }
        public void WriteArray(byte[] array)
        {
            WriteVarInt(array.Length);
            WriteData(array);
        }
        public void WriteBool(bool b)
        {
            WriteByte((byte)(b ? 1 : 0));
        }
        public Stream GetStream()
        {
            return this.s;
        }
        public byte[] ToArray()
        {
            return ((MemoryStream)s).ToArray();
        }

        public void Dispose()
        {
            s.Close();
        }
    }
}
