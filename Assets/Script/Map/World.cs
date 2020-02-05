using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Cubecraft.Data.World;

public class World : MonoBehaviour
{
    // 用来管理 chunk
    public Dictionary<Vector2Int, Column> chunks = new Dictionary<Vector2Int, Column>();
    // chunk 预设体，用做创建对象的模板
    public GameObject columnPrefab;

    void Start()
    {
        Global.blockDic.RegisterAll();
    }

    /// <summary>
    /// 创建 chunk
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void CreateColumn(ChunkColumn column)
    {
        Vector3 pos = new Vector3(column.ChunkX, 0, column.ChunkZ);
        GameObject newColumnObject = Instantiate(columnPrefab, pos, Quaternion.Euler(Vector3.zero));

        Column newColumn = newColumnObject.GetComponent<Column>();
        chunks.Add(new Vector2Int(column.ChunkX, column.ChunkZ), newColumn);
        newColumn.world = this;
        StartCoroutine(newColumn.CreateColumn(column.ChunkX, column.ChunkZ, column));
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
        Column column = null;
        if (chunks.TryGetValue(new Vector2Int(x, z), out column))
        {
            Object.Destroy(column.gameObject);
            chunks.Remove(new Vector2Int(x, z));
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
