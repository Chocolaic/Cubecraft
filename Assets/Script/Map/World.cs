using UnityEngine;
using System.Collections.Generic;
using Cubecraft.Data.World;
using System.Threading;
using System.Collections.Concurrent;

public class World : MonoBehaviour
{
    // 用来管理 chunk
    public Dictionary<Vector2Int, Column> chunks = new Dictionary<Vector2Int, Column>();
    public BlockingCollection<Chunk> calculateQueue = new BlockingCollection<Chunk>(new ConcurrentQueue<Chunk>());
    // chunk 预设体，用做创建对象的模板
    public GameObject columnPrefab;
    private Thread calculateThread = null;
    private bool onhandle;
    void Start()
    {
        Global.blockDic.RegisterAll();
        onhandle = true;
        //单独线程来计算区块剔除面
        calculateThread = new Thread(() =>
        {
            while (onhandle)
            {
                Chunk chunk = calculateQueue.Take();
                chunk.UpdateChunk();
            }
        })
        { IsBackground = true };
        calculateThread.Start();
    }
    void OnDestroy()
    {
        onhandle = false;
        calculateThread.Abort();
    }
    /// <summary>
    /// 创建 chunk
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void CreateColumn(ChunkColumn column)
    {
        Vector2Int dicpos = new Vector2Int(column.ChunkX, column.ChunkZ);
        if (chunks.ContainsKey(dicpos))
            DestroyColumn(dicpos);//重新载入区块
        Vector3 worldpos = new Vector3(column.ChunkX, 0, column.ChunkZ);
        GameObject newColumnObject = Instantiate(columnPrefab, worldpos, Quaternion.Euler(Vector3.zero));

        Column newColumn = newColumnObject.GetComponent<Column>();
        chunks.Add(dicpos, newColumn);
        newColumn.world = this;
        StartCoroutine(newColumn.CreateColumn(dicpos, column));
    }

    /// <summary>
    /// 销毁指定的 chunk
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void DestroyColumn(int x, int z)
    {
        // 逻辑简单，就是找到指定的 chunk，然后先销毁游戏对象，再移除管理即可（移除对应字典的值）
        DestroyColumn(new Vector2Int(x, z));
    }
    public void DestroyColumn(Vector2Int pos)
    {
        Column column = null;
        if (chunks.TryGetValue(pos, out column))
        {
            Object.Destroy(column.gameObject);
            chunks.Remove(pos);
        }
    }

    /// <summary>
    /// 根据传入的位置，获取指定的 column
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Column GetColumn(int x, int z)
    {
        Column column = null;

        Vector2Int pos = new Vector2Int(x, z);

        chunks.TryGetValue(pos, out column);
        return column;
    }
}
