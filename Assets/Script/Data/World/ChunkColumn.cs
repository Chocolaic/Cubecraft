using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World
{
    public class ChunkColumn
    {
        public const int ColumnSize = 16;

        public int ChunkX { get; private set; }
        public int ChunkZ { get; private set; }
        private readonly ChunkData[] chunks = new ChunkData[ColumnSize];
        public ChunkData this[int chunkY]
        {
            get
            {
                return chunks[chunkY];
            }
            set
            {
                chunks[chunkY] = value;
            }
        }
        public ChunkColumn(int x, int z)
        {
            this.ChunkX = x;
            this.ChunkZ = z;
        }
    }
}
