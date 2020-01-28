using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private Dictionary<int, Dictionary<int, ChunkColumn>> chunks = new Dictionary<int, Dictionary<int, ChunkColumn>>();
    private Dictionary<int, Block> BlockDic = new Dictionary<int, Block>();

    public void RegisterBlock(int key, Block block)
    {
        BlockDic.Add(key, block);
    }
    public bool UnregisterBlock(int key)
    {
        if (BlockDic.ContainsKey(key))
        {
            BlockDic.Remove(key);
            return true;
        }
        else
            return false;
    }
    public Block SetBlock(Block block, Vector3 pos)
    {
        Block newInstance = Instantiate(block, pos, Quaternion.identity);
        return newInstance;
    }

    public Block SetBlock(int key, Vector3 pos)
    {
        if (!BlockDic.ContainsKey(key))
            key = 1;
        return Instantiate(BlockDic[key], pos, Quaternion.identity);
    }
    public ChunkColumn this[int chunkX, int chunkZ]
    {
        get
        {
            //Read a chunk
            if (chunks.ContainsKey(chunkX))
                if (chunks[chunkX].ContainsKey(chunkZ))
                    return chunks[chunkX][chunkZ];
            return null;
        }
        set
        {
            if (value != null)
            {
                //Update a chunk column
                if (!chunks.ContainsKey(chunkX))
                    chunks[chunkX] = new Dictionary<int, ChunkColumn>();
                chunks[chunkX][chunkZ] = value;
            }
            else
            {
                //Unload a chunk column
                if (chunks.ContainsKey(chunkX))
                {
                    if (chunks[chunkX].ContainsKey(chunkZ))
                    {
                        chunks[chunkX].Remove(chunkZ);
                        if (chunks[chunkX].Count == 0)
                            chunks.Remove(chunkX);
                    }
                }
            }
        }
    }
}
