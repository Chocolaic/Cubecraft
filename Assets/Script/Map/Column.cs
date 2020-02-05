using Cubecraft.Data.World;
using Cubecraft.Data.World.Blocks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    int size = 0;
    private Chunk[] chunks = new Chunk[16];
    public GameObject chunkPrefab;
    public World world;
    public int posX, posZ;

    private ChunkColumn column;
    public IEnumerator CreateColumn(int x, int z, ChunkColumn column)
    {
        this.column = column;
        this.posX = x;
        this.posZ = z;
        for(int chunkY = 0; chunkY < ChunkColumn.ColumnSize; chunkY++)
        {
            Vector3 worldPos = new Vector3(x * 16, chunkY * 16, z * 16);
            ChunkData chunk = column[chunkY];
            size++;
            GameObject newChunkObject = Instantiate(chunkPrefab, worldPos, Quaternion.Euler(Vector3.zero));
            newChunkObject.transform.SetParent(gameObject.transform);

            Chunk newChunk = newChunkObject.GetComponent<Chunk>();
            newChunk.position = chunkY;
            newChunk.column = this;
            newChunk.chunkData = chunk;
            chunks[chunkY] = newChunk;
            if (chunk != null)
            {
                newChunk.chunkSize = 16;
                for (int blockX = 0; blockX < newChunk.chunkSize; blockX++)
                {
                    for (int blockY = 0; blockY < newChunk.chunkSize; blockY++)
                    {
                        for (int blockZ = 0; blockZ < newChunk.chunkSize; blockZ++)
                        {
                            newChunk.SetBlock(blockX, blockY, blockZ, Global.blockDic.Instantiate(chunk[blockX, blockY, blockZ].ID));
                        }
                    }
                }
            }
            newChunk.DoUpdate();
            yield return null;
        }
    }
    public BlockState GetBlock(int chunkPos, int blockX, int blockY, int blockZ)
    {     
        if (blockX < 0 || blockX > 15)
        {
            int offsetX = blockX % 15;
            return world.GetColumn(posX + offsetX, posZ).GetBlock(chunkPos, blockX - offsetX * 16, blockY, blockZ);
        }else if(blockZ < 0 || blockZ > 15)
        {
            int offsetZ = blockZ % 15;
            return world.GetColumn(posX, posZ + offsetZ).GetBlock(chunkPos, blockX, blockY, blockZ - offsetZ * 16);
        }
        else if (blockY < 0 || blockY > 15)
        {
            int offsetY = blockY % 15;
            return GetBlock(chunkPos + offsetY, blockX, blockY - offsetY * 16, blockZ);
        }
        else if (chunkPos < ChunkColumn.ColumnSize && chunkPos >= 0)
        {
            ChunkData targetChunk = column[chunkPos];
            return targetChunk != null ? targetChunk[blockX, blockY, blockZ] : BlockStorage.AIR;
        }
        return BlockStorage.AIR;
    }
    public Chunk this[int index]
    {
        get { return chunks[index]; }
    }
}
