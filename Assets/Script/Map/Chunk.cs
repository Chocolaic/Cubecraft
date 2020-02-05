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

    private Block[,,] blocks = new Block[16, 16, 16]; //用于存放数据块的方块的数据
    public int chunkSize = 0;  //设置数据块的大小
    private bool update = false;  //一个标志位，用于判断该数据块是否已经更新
    private bool[] updateComplete = new bool[4];

    MeshFilter filter;
    MeshCollider coll;
    MeshData meshData = new MeshData();
    public Column column;
    public ChunkData chunkData;
    public int position;

    void Awake()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
        if (updateComplete[0] && updateComplete[1] && updateComplete[2] && updateComplete[3])
        {
            RenderMesh(meshData);
            updateComplete[0] = false;
            updateComplete[1] = false;
            updateComplete[2] = false;
            updateComplete[3] = false;
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
        meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshVertical(this, x, y, z, meshData);
                }
            }
        }

        Column targetColumn = null; Chunk targetChunk = null;
        if ((targetColumn = column.world.GetColumn(column.posX + 1, column.posZ)) != null)
        {
            if ((targetChunk = targetColumn[position]) != null)
                targetChunk.UpdateLeft();
            UpdateRight();
        }
        if ((targetColumn = column.world.GetColumn(column.posX - 1, column.posZ)) != null)
        {
            if ((targetChunk = targetColumn[position]) != null)
                targetChunk.UpdateRight();
            UpdateLeft();
        }
        if ((targetColumn = column.world.GetColumn(column.posX, column.posZ + 1)) != null)
        {
            if ((targetChunk = targetColumn[position]) != null)
                targetChunk.UpdateFront();
            UpdateBack();
        }
        if ((targetColumn = column.world.GetColumn(column.posX, column.posZ - 1)) != null)
        {
            if ((targetChunk = targetColumn[position]) != null)
                targetChunk.UpdateBack();
            UpdateFront();
        }
    }

    public  void UpdateLeft()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshLeft(this, x, y, z, meshData);
                }
            }
        }
        updateComplete[0] = true;
    }
    public void UpdateRight()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshRight(this, x, y, z, meshData);
                }
            }
        }
        updateComplete[1] = true;
    }
    public void UpdateFront()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshFront(this, x, y, z, meshData);
                }
            }
        }
        updateComplete[2] = true;
    }
    public void UpdateBack()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshBack (this, x, y, z, meshData);
                }
            }
        }
        updateComplete[3] = true;
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
