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
                for (int chunkY = 0; chunkY < ChunkColumn.ColumnSize; chunkY++)
                {
                    if ((chunkMask & (1 << chunkY)) != 0)
                    {
                        byte bitsPerBlock = (byte)input.ReadByte();
                        bool usePalette = (bitsPerBlock <= 8);
                        // Vanilla Minecraft will use at least 4 bits per block
                        if (bitsPerBlock < 4)
                            bitsPerBlock = 4;
                        int paletteLength = input.ReadVarInt();

                        int[] palette = new int[paletteLength];
                        for (int i = 0; i < paletteLength; i++)
                            palette[i] = input.ReadVarInt();
                        // Bit mask covering bitsPerBlock bits
                        // EG, if bitsPerBlock = 5, valueMask = 00011111 in binary
                        uint valueMask = (uint)((1 << bitsPerBlock) - 1);

                        ulong[] dataArray = input.ReadULongArray();

                        ChunkData chunk = new ChunkData();
                        if (dataArray.Length > 0)
                        {
                            for (int blockY = 0; blockY < ChunkData.SizeY; blockY++)
                            {
                                for (int blockZ = 0; blockZ < ChunkData.SizeZ; blockZ++)
                                {
                                    for (int blockX = 0; blockX < ChunkData.SizeX; blockX++)
                                    {
                                        int blockNumber = (blockY * ChunkData.SizeZ + blockZ) * ChunkData.SizeX + blockX;

                                        int startLong = (blockNumber * bitsPerBlock) / 64;
                                        int startOffset = (blockNumber * bitsPerBlock) % 64;
                                        int endLong = ((blockNumber + 1) * bitsPerBlock - 1) / 64;
                                        // TODO: In the future a single ushort may not store the entire block id;
                                        // the Block code may need to change if block state IDs go beyond 65535
                                        ushort blockId;
                                        if (startLong == endLong)
                                            blockId = (ushort)((dataArray[startLong] >> startOffset) & valueMask);
                                        else
                                        {
                                            int endOffset = 64 - startOffset;
                                            blockId = (ushort)((dataArray[startLong] >> startOffset | dataArray[endLong] << endOffset) & valueMask);
                                        }
                                        if (usePalette)
                                        {
                                            // Get the real block ID out of the palette
                                            blockId = (ushort)palette[blockId];
                                        }

                                        chunk[blockX, blockY, blockZ] = new AbstractBlock(blockId);
                                    }
                                }
                            }
                        }
                        column[chunkY] = chunk;
                    }
                }
                return column;
            }
        }
    }
}
