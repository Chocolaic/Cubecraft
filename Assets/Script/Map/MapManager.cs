using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private Dictionary<BlockType, Block> BlockDic = new Dictionary<BlockType, Block>();

    public void RegisterBlock(BlockType key, Block block)
    {
        BlockDic.Add(key, block);
    }
    public bool UnregisterBlock(BlockType key)
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

    public Block SetBlock(BlockType key, Vector3 pos)
    {
        return Instantiate(BlockDic[key], pos, Quaternion.identity);
    }
}
