using Cubecraft.Data.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    int size = 0;
    private List<Chunk> chunks = new List<Chunk>();
    public GameObject chunkPrefab;
    public World world;
    public int posX, posZ;

    private ChunkColumn column;
    public IEnumerator CreateColumn(int x, int z, ChunkColumn column)
    {
        this.column = column;
        for(int chunkY = 0; chunkY < ChunkColumn.ColumnSize; chunkY++)
        {
            Vector3 worldPos = new Vector3(x * 16, chunkY * 16, z * 16);
            ChunkData chunk = column[chunkY];
            size++;
            if (chunk == null)
                break;
            GameObject newChunkObject = Instantiate(chunkPrefab, worldPos, Quaternion.Euler(Vector3.zero));
            newChunkObject.transform.SetParent(gameObject.transform);

            Chunk newChunk = newChunkObject.GetComponent<Chunk>();
            newChunk.position = chunkY;
            newChunk.column = this;
            newChunk.chunkData = chunk;
            chunks.Add(newChunk);

            for (int blockX = 0; blockX < Chunk.chunkSize; blockX++)
            {
                for (int blockY = 0; blockY < Chunk.chunkSize; blockY++)
                {
                    for (int blockZ = 0; blockZ < Chunk.chunkSize; blockZ++)
                    {
                        if (chunk[blockX, blockY, blockZ].ID > 0)
                            newChunk.SetBlock(blockX, blockY, blockZ, new GrassBlock());
                        else
                            newChunk.SetBlock(blockX, blockY, blockZ, new AirBlock());
                    }
                }
            }
            newChunk.DoUpdate();
            yield return null;
        }
    }
    public BlockState GetBlock(int chunkPos, int blockX, int blockY, int blockZ)
    {     
        if (blockX < 0 || blockX > 15 || blockZ < 0 || blockZ > 15)
            return new BlockState(1, 0);
        else if (blockY > 15)
            return GetBlock(chunkPos + 1, blockX, blockY - 16, blockZ);
        else if (blockY < 0)
            return GetBlock(chunkPos - 1, blockX, blockY + 16, blockZ);
        else if (chunkPos < ChunkColumn.ColumnSize && chunkPos >= 0)
        {
            ChunkData targetChunk = column[chunkPos];
            return targetChunk != null ? targetChunk[blockX, blockY, blockZ] : BlockStorage.AIR;
        }
        return BlockStorage.AIR;
    }
    public void ChunkFrontMesh()
    {

    }
}
