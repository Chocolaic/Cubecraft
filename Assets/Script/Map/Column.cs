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
    public IEnumerator CreateColumn(int x, int z, ChunkColumn column)
    {
        this.posX = x;
        this.posZ = z;
        Chunk newChunk = null;
        ChunkData chunk = null;
        for (int chunkY = 0; chunkY < ChunkColumn.ColumnSize; chunkY++)
        {
            Vector3 worldPos = new Vector3(x * 16, chunkY * 16, z * 16);
            chunk = column[chunkY];
            size++;
            GameObject newChunkObject = Instantiate(chunkPrefab, worldPos, Quaternion.Euler(Vector3.zero));
            newChunkObject.transform.SetParent(gameObject.transform);

            newChunk = newChunkObject.GetComponent<Chunk>();
            newChunk.position = chunkY;
            newChunk.column = this;
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
                            newChunk.SetBlock(blockX, blockY, blockZ, Global.blockDic.GetBlock(chunk[blockX, blockY, blockZ].ID));
                        }
                    }
                }
            }
            world.calculateQueue.Add(newChunk);
            yield return null;
        }
        System.GC.Collect();
    }
    public Chunk this[int index]
    {
        get { return chunks[index]; }
    }
}
