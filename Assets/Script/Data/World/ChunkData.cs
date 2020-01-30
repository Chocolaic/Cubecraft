using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World
{
    public class ChunkData
    {
        public BlockStorage Blocks { get; private set; }
        public ChunkData(BlockStorage blocks)
        {
            this.Blocks = blocks;
        }
        public BlockState this[int x, int y, int z]
        {
            get
            {
                return this.Blocks[x, y, z];
            }
        }
    }
}
