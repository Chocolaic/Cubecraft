using Cubecraft.Net.Protocol;
using Cubecraft.Net.Protocol.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World
{
    public class BlockStorage
    {
        public static BlockState AIR = new BlockState(0, 0);
        int bitsPerEntry;
        BlockState[] states;
        FlexibleStorage storage;
        public BlockStorage(InputBuffer input)
        {
            this.bitsPerEntry = input.ReadByte();
            int stateCount = input.ReadVarInt();
            this.states = new BlockState[stateCount];
            for (int i = 0; i < stateCount; i++)
            {
                this.states[i] = NetUtils.ReadBlockState(input);
            }
            this.storage = new FlexibleStorage(this.bitsPerEntry, input.ReadULongArray());
        }
        private static int Index(int x, int y,int z)
        {
            return y << 8 | z << 4 | x;
        }
        public BlockState this[int x, int y, int z]
        {
            get
            {
                int id = this.storage[Index(x, y, z)];
                return this.bitsPerEntry <= 8 ? (id >= 0 && id < this.states.Length ? this.states[id] : AIR) : RawToState(id);
            }
        }
        public BlockState RawToState(int raw)
        {
            return new BlockState(raw >> 4, raw & 0xF);
        }
    }
}
