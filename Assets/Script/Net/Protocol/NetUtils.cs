using Cubecraft.Data.World;
using Cubecraft.Net.Protocol.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Net.Protocol
{
    class NetUtils
    {
        public static ChunkColumn ReadColumnData(byte[] data, int chunkX, int chunkZ, bool chunksContinuous, bool hasSkyLight, int chunkMask)
        {
            using (MemoryStream memstream = new MemoryStream(data, false))
            {
                InputBuffer input = new InputBuffer(memstream);
                ChunkColumn column = new ChunkColumn(chunkX, chunkZ);
                for (int chunkY = 0; chunkY < 16; chunkY++)
                {
                    if ((chunkMask & (1 << chunkY)) != 0)
                    {
                        BlockStorage blocks = new BlockStorage(input);
                        input.ReadData(2048);
                        if (hasSkyLight)
                            input.ReadData(2048);
                        column[chunkY] = new ChunkData(blocks);
                    }
                }
                return column;
            }
        }
        public static BlockState ReadBlockState(InputBuffer input)
        {
            int rawId = input.ReadVarInt();
            return new BlockState(rawId >> 4, rawId & 0xF);
        }
    }
}
