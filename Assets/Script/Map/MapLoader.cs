using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MapManager
{
    private float relief = 15.0f;
    private float seedX, seedZ;

    [SerializeField]
    public GameObject player;
    private int maxHeight = 10;
    public int size;
    public int depth;

    // Start is called before the first frame update
    void Start()
    {
        RegisterAll();
        CreaterNewWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RegisterAll()
    {
        RegisterBlock(BlockType.Stone, LoadBlock("block_stone"));
        RegisterBlock(BlockType.Grass, LoadBlock("block_grass"));
        RegisterBlock(BlockType.Dirt, LoadBlock("block_dirt"));
    }

    Block LoadBlock(string name)
    {
        return Resources.Load<Block>("prefabs/blocks/" + name);
    }

    public void CreaterNewWorld()
    {
        GameObject me = player;

        MapLoader world = this;
        /*==============*/
        world.execute(me);
        /*===============*/
    }

    void execute(GameObject player)
    {
        float y = 0;
        float y1 = 0;
        for (int i = 0; i < this.size; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                float xSample1 = (i + seedX) / relief;
                float zSample1 = (j + seedZ) / relief;
                float noise1 = Mathf.PerlinNoise(xSample1, zSample1);
                y1 = maxHeight * noise1;
                // 为了模仿我的世界的格子风 将每一次计算出来的浮点数值转换到整数值
                y1 = Mathf.Round(y1);
                Block b = null;
                if (y1 > maxHeight * 0.3f)
                {
                    b = SetBlock(BlockType.Grass, new Vector3(i, y, j));
                }
                else if (y1 > maxHeight * 0.1f)
                {
                    b = SetBlock(BlockType.Stone, new Vector3(i, y, j));
                }
                else
                {
                    b = SetBlock(BlockType.Dirt, new Vector3(i, y, j));
                }
                float xSample = (b.transform.localPosition.x + seedX) / relief;
                float zSample = (b.transform.localPosition.z + seedZ) / relief;
                float noise = Mathf.PerlinNoise(xSample, zSample);
                y = maxHeight * noise;
                // 为了模仿我的世界的格子风 将每一次计算出来的浮点数值转换到整数值
                y = Mathf.Round(y);
                b.transform.localPosition = new Vector3(b.transform.localPosition.x, y, b.transform.localPosition.z);
            }
        }
        GameObject p = Instantiate(player);
        p.transform.position = new Vector3(size / 2, 20, size / 2);
        Debug.Log("completed");
    }
}
