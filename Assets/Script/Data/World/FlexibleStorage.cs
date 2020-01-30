using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World
{
    class FlexibleStorage
    {
        ulong[] data;
        int bitsPerEntry;
        int size;
        ulong maxEntryValue;
        public FlexibleStorage(int bitsPerEntry, ulong[] data)
        {
            if (bitsPerEntry < 4)
                bitsPerEntry = 4;
            this.bitsPerEntry = bitsPerEntry;
            this.data = data;
            this.size = this.data.Length * 64 / this.bitsPerEntry;
            this.maxEntryValue = (ulong)(1L << this.bitsPerEntry) - 1;
        }
        public int this[int index]
        {
            get
            {
                if (index < 0 || index > this.size - 1)
                    throw new IndexOutOfRangeException((this.size -1) + "data.Length " + this.data.Length);
                int bitIndex = index * this.bitsPerEntry;
                int startIndex = bitIndex / 64;
                int endIndex = ((index + 1) * this.bitsPerEntry - 1) / 64;
                int startBitSubIndex = bitIndex % 64;
                if (startIndex == endIndex)
                    return (int)(this.data[startIndex] >> startBitSubIndex & this.maxEntryValue);
                else
                {
                    int endBitSubIndex = 64 - startBitSubIndex;
                    return (int)((this.data[startIndex] >> startBitSubIndex | this.data[endIndex] << endBitSubIndex) & this.maxEntryValue);
                }
            }
        }
    }
}
