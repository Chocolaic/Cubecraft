using Cubecraft.Data.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{

    private Block[,,] blocks; //用于存放数据块的方块的数据
    public static int chunkSize = 16;  //设置数据块的大小
    private bool update = false;  //一个标志位，用于判断该数据块是否已经更新

    MeshFilter filter;
    MeshCollider coll;

    public Column column;
    public ChunkData chunkData;
    public int position;

    void Awake()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
        blocks = new Block[chunkSize, chunkSize, chunkSize];
    }

    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }

    /// <summary>
    /// 用于获取对应位置的方块信息
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public BlockState GetBlock(int x, int y, int z)
    {
        return column.GetBlock(position, x, y, z);
    }
    public Block this[int x, int y, int z]
    {
        get { return blocks[x, y, z]; }
    }

    /// <summary>
    /// 设置 Block，并做相关应检测
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="block"></param>
    public void SetBlock(int x, int y, int z, Block block)
    {
        blocks[x, y, z] = block;
    }

    /// <summary>
    /// 用于在每一帧中更新网格数据
    /// </summary>
    void UpdateChunk()
    {
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshData(this, x, y, z, meshData);
                }
            }
        }

        RenderMesh(meshData);
    }

    public void DoUpdate()
    {
        this.update = true;
    }

    /// <summary>
    /// 用于在每一帧中渲染网格数据
    /// </summary>
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

}
