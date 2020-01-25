using Cubecraft.Net.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cubecraft.Net.Protocol
{
    class DataTypes
    {
        private int protocolversion;

        public DataTypes(int protocol)
        {
            this.protocolversion = protocol;
        }
        public byte[] ReadData(int offset, List<byte> cache)
        {
            byte[] result = cache.Take(offset).ToArray();
            cache.RemoveRange(0, offset);
            return result;
        }

        public string ReadNextString(List<byte> cache)
        {
            int length = ReadNextVarInt(cache);
            if (length > 0)
            {
                return Encoding.UTF8.GetString(ReadData(length, cache));
            }
            else return "";
        }
        public bool ReadNextBool(List<byte> cache)
        {
            return ReadNextByte(cache) != 0x00;
        }
        public short ReadNextShort(List<byte> cache)
        {
            byte[] rawValue = ReadData(2, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt16(rawValue, 0);
        }
        public int ReadNextInt(List<byte> cache)
        {
            byte[] rawValue = ReadData(4, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt32(rawValue, 0);
        }
        public long ReadNextLong(List<byte> cache)
        {
            byte[] rawValue = ReadData(8, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToInt64(rawValue, 0);
        }
        public ushort ReadNextUShort(List<byte> cache)
        {
            byte[] rawValue = ReadData(2, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToUInt16(rawValue, 0);
        }
        public ulong ReadNextULong(List<byte> cache)
        {
            byte[] rawValue = ReadData(8, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToUInt64(rawValue, 0);
        }
        public Vector3 ReadNextLocation(List<byte> cache)
        {
            ulong locEncoded = ReadNextULong(cache);
            int x, y, z;
            if (protocolversion >= CubeProtocol.MC114Version)
            {
                x = (int)(locEncoded >> 38);
                y = (int)(locEncoded & 0xFFF);
                z = (int)(locEncoded << 26 >> 38);
            }
            else
            {
                x = (int)(locEncoded >> 38);
                y = (int)((locEncoded >> 26) & 0xFFF);
                z = (int)(locEncoded << 38 >> 38);
            }
            if (x >= 33554432)
                x -= 67108864;
            if (y >= 2048)
                y -= 4096;
            if (z >= 33554432)
                z -= 67108864;
            return new Vector3(x, y, z);
        }
        public ushort[] ReadNextUShortsLittleEndian(int amount, List<byte> cache)
        {
            byte[] rawValues = ReadData(2 * amount, cache);
            ushort[] result = new ushort[amount];
            for (int i = 0; i < amount; i++)
                result[i] = BitConverter.ToUInt16(rawValues, i * 2);
            return result;
        }
        public Guid ReadNextUUID(List<byte> cache)
        {
            byte[] javaUUID = ReadData(16, cache);
            Guid guid;
            if (BitConverter.IsLittleEndian)
            {
                // Convert big-endian Java UUID to little-endian .NET GUID
                byte[] netGUID = new byte[16];
                for (int i = 8; i < 16; i++)
                    netGUID[i] = javaUUID[i];
                netGUID[3] = javaUUID[0];
                netGUID[2] = javaUUID[1];
                netGUID[1] = javaUUID[2];
                netGUID[0] = javaUUID[3];
                netGUID[5] = javaUUID[4];
                netGUID[4] = javaUUID[5];
                netGUID[6] = javaUUID[7];
                netGUID[7] = javaUUID[6];
                guid = new Guid(netGUID);
            }
            else
            {
                guid = new Guid(javaUUID);
            }
            return guid;
        }
        public byte[] ReadNextByteArray(List<byte> cache)
        {
            int len = protocolversion >= CubeProtocol.MC18Version
                ? ReadNextVarInt(cache)
                : ReadNextShort(cache);
            return ReadData(len, cache);
        }
        public ulong[] ReadNextULongArray(List<byte> cache)
        {
            int len = ReadNextVarInt(cache);
            ulong[] result = new ulong[len];
            for (int i = 0; i < len; i++)
                result[i] = ReadNextULong(cache);
            return result;
        }
        public double ReadNextDouble(List<byte> cache)
        {
            byte[] rawValue = ReadData(8, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToDouble(rawValue, 0);
        }
        public float ReadNextFloat(List<byte> cache)
        {
            byte[] rawValue = ReadData(4, cache);
            Array.Reverse(rawValue); //Endianness
            return BitConverter.ToSingle(rawValue, 0);
        }
        public int ReadNextVarIntRAW(SocketWrapper socket)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (true)
            {
                k = socket.ReadDataRAW(1)[0];
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }
        public int ReadNextVarInt(List<byte> cache)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (true)
            {
                k = ReadNextByte(cache);
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }
        public int ReadNextVarShort(List<byte> cache)
        {
            ushort low = ReadNextUShort(cache);
            byte high = 0;
            if ((low & 0x8000) != 0)
            {
                low &= 0x7FFF;
                high = ReadNextByte(cache);
            }
            return ((high & 0xFF) << 15) | low;
        }
        public byte ReadNextByte(List<byte> cache)
        {
            byte result = cache[0];
            cache.RemoveAt(0);
            return result;
        }
        public Dictionary<string, object> ReadNextNbt(List<byte> cache)
        {
            return ReadNextNbt(cache, true);
        }
        private Dictionary<string, object> ReadNextNbt(List<byte> cache, bool root)
        {
            if (root)
            {
                if (cache[0] != 10) // TAG_Compound
                    throw new System.IO.InvalidDataException("Failed to decode NBT: Does not start with TAG_Compound");
                ReadNextByte(cache); // Tag type (TAG_Compound)
                ReadData(ReadNextUShort(cache), cache); // NBT root name
            }

            Dictionary<string, object> NbtData = new Dictionary<string, object>();

            while (true)
            {
                int fieldType = ReadNextByte(cache);

                if (fieldType == 0) // TAG_End
                    return NbtData;

                int fieldNameLength = ReadNextUShort(cache);
                string fieldName = Encoding.ASCII.GetString(ReadData(fieldNameLength, cache));
                object fieldValue = ReadNbtField(cache, fieldType);

                // This will override previous tags with the same name
                NbtData[fieldName] = fieldValue;
            }
        }
        private object ReadNbtField(List<byte> cache, int fieldType)
        {
            switch (fieldType)
            {
                case 1: // TAG_Byte
                    return ReadNextByte(cache);
                case 2: // TAG_Short
                    return ReadNextShort(cache);
                case 3: // TAG_Int
                    return ReadNextInt(cache);
                case 4: // TAG_Long
                    return ReadNextLong(cache);
                case 5: // TAG_Float
                    return ReadNextFloat(cache);
                case 6: // TAG_Double
                    return ReadNextDouble(cache);
                case 7: // TAG_Byte_Array
                    return ReadData(ReadNextInt(cache), cache);
                case 8: // TAG_String
                    return Encoding.UTF8.GetString(ReadData(ReadNextUShort(cache), cache));
                case 9: // TAG_List
                    int listType = ReadNextByte(cache);
                    int listLength = ReadNextInt(cache);
                    object[] listItems = new object[listLength];
                    for (int i = 0; i < listLength; i++)
                        listItems[i] = ReadNbtField(cache, listType);
                    return listItems;
                case 10: // TAG_Compound
                    return ReadNextNbt(cache, false);
                case 11: // TAG_Int_Array
                    cache.Insert(0, 3);             // List type = TAG_Int
                    return ReadNbtField(cache, 9);  // Read as TAG_List
                case 12: // TAG_Long_Array
                    cache.Insert(0, 4);             // List type = TAG_Long
                    return ReadNbtField(cache, 9);  // Read as TAG_List
                default:
                    throw new System.IO.InvalidDataException("Failed to decode NBT: Unknown field type " + fieldType);
            }
        }
        public byte[] GetVarInt(int paramInt)
        {
            List<byte> bytes = new List<byte>();
            while ((paramInt & -128) != 0)
            {
                bytes.Add((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            bytes.Add((byte)paramInt);
            return bytes.ToArray();
        }
        public byte[] GetDouble(double number)
        {
            byte[] theDouble = BitConverter.GetBytes(number);
            Array.Reverse(theDouble); //Endianness
            return theDouble;
        }
        public byte[] GetFloat(float number)
        {
            byte[] theFloat = BitConverter.GetBytes(number);
            Array.Reverse(theFloat); //Endianness
            return theFloat;
        }
        public byte[] GetArray(byte[] array)
        {
            if (protocolversion < CubeProtocol.MC18Version)
            {
                byte[] length = BitConverter.GetBytes((short)array.Length);
                Array.Reverse(length);
                return ConcatBytes(length, array);
            }
            else return ConcatBytes(GetVarInt(array.Length), array);
        }
        public byte[] GetString(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            return ConcatBytes(GetVarInt(bytes.Length), bytes);
        }
        public byte[] GetLocation(Vector3 location)
        {
            if (protocolversion >= CubeProtocol.MC114Version)
            {
                return BitConverter.GetBytes(((((ulong)location.x) & 0x3FFFFFF) << 38) | ((((ulong)location.z) & 0x3FFFFFF) << 12) | (((ulong)location.y) & 0xFFF));
            }
            else return BitConverter.GetBytes(((((ulong)location.x) & 0x3FFFFFF) << 38) | ((((ulong)location.y) & 0xFFF) << 26) | (((ulong)location.z) & 0x3FFFFFF));
        }
        public byte[] ConcatBytes(params byte[][] bytes)
        {
            List<byte> result = new List<byte>();
            foreach (byte[] array in bytes)
                result.AddRange(array);
            return result.ToArray();
        }
        public int Atoi(string str)
        {
            return int.Parse(new string(str.Trim().TakeWhile(char.IsDigit).ToArray()));
        }
    }
}
