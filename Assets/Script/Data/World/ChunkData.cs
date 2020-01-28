using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkData
{
    public const int SizeX = 16;
    public const int SizeY = 16;
    public const int SizeZ = 16;

    private readonly IBlock[,,] blocks = new AbstractBlock[SizeX, SizeY, SizeZ];

    public IBlock this[int blockX, int blockY, int blockZ]
    {
        get
        {
            if (blockX < 0 || blockX >= SizeX)
                throw new ArgumentOutOfRangeException("blockX", "Must be between 0 and " + (SizeX - 1) + " (inclusive)");
            if (blockY < 0 || blockY >= SizeY)
                throw new ArgumentOutOfRangeException("blockY", "Must be between 0 and " + (SizeY - 1) + " (inclusive)");
            if (blockZ < 0 || blockZ >= SizeZ)
                throw new ArgumentOutOfRangeException("blockZ", "Must be between 0 and " + (SizeZ - 1) + " (inclusive)");
            return blocks[blockX, blockY, blockZ];
        }
        set
        {
            if (blockX < 0 || blockX >= SizeX)
                throw new ArgumentOutOfRangeException("blockX", "Must be between 0 and " + (SizeX - 1) + " (inclusive)");
            if (blockY < 0 || blockY >= SizeY)
                throw new ArgumentOutOfRangeException("blockY", "Must be between 0 and " + (SizeY - 1) + " (inclusive)");
            if (blockZ < 0 || blockZ >= SizeZ)
                throw new ArgumentOutOfRangeException("blockZ", "Must be between 0 and " + (SizeZ - 1) + " (inclusive)");
            blocks[blockX, blockY, blockZ] = value;
        }
    }
    public IBlock GetBlock(Vector3 location)
    {
        return this[(int)location.x, (int)location.y, (int)location.z];
    }
}
