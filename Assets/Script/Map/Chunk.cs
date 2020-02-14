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
    private bool onload = false;  //一个标志位，用于判断该数据块是否已经更新

    MeshFilter filter;
    MeshCollider coll;
    MeshData meshData = new MeshData();
    public Column column;
    public int position;
    public Chunk FrontChunk { get; set; }
    public Chunk BackChunk { get; set; }
    public Chunk LeftChunk { get; set; }
    public Chunk RightChunk { get; set; }
    public Chunk UpChunk { get; set; }
    public Chunk DownChunk { get; set; }
    private bool[] updateComplete = new bool[5];

    void Awake()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }
    void LateUpdate()
    {
        if (updateComplete[0] && updateComplete[1] && updateComplete[2] && updateComplete[3] && updateComplete[4])
        {
            RenderMesh();
            updateComplete[0] = false;
            updateComplete[1] = false;
            updateComplete[2] = false;
            updateComplete[3] = false;
            updateComplete[4] = false;
        }
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
    public void UpdateChunk()
    {
        meshData = new MeshData();
        Column targetColumn = null; Chunk targetChunk = null;

        if (position == 0)
        {
            UpdateDown();
        }
        else if ((targetChunk = column[position - 1]) != null)
        {
            targetChunk.UpChunk = this;
            this.DownChunk = targetChunk;
            targetChunk.UpdateUp();
            UpdateDown();
        }
        if (position == 15)
        {
            UpdateUp();
        }
        else if ((targetChunk = column[position + 1]) != null)
        {
            targetChunk.DownChunk = this;
            this.UpChunk = targetChunk;
            targetChunk.UpdateDown();
            UpdateUp();
        }
        if ((targetColumn = column.world.GetColumn(column.posX + 1, column.posZ)) != null && (targetChunk = targetColumn[position]) != null)
        {
            targetChunk.LeftChunk = this;
            this.RightChunk = targetChunk;
            targetChunk.UpdateLeft();
            UpdateRight();
        }
        if ((targetColumn = column.world.GetColumn(column.posX - 1, column.posZ)) != null && (targetChunk = targetColumn[position]) != null)
        {
            targetChunk.RightChunk = this;
            this.LeftChunk = targetChunk;
            targetChunk.UpdateRight();
            UpdateLeft();
        }
        if ((targetColumn = column.world.GetColumn(column.posX, column.posZ + 1)) != null && (targetChunk = targetColumn[position]) != null)
        {
            targetChunk.FrontChunk = this;
            this.BackChunk = targetChunk;
            targetChunk.UpdateFront();
            UpdateBack();
        }
        if ((targetColumn = column.world.GetColumn(column.posX, column.posZ - 1)) != null && (targetChunk = targetColumn[position]) != null)
        {
            targetChunk.BackChunk = this;
            this.FrontChunk = targetChunk;
            targetChunk.UpdateBack();
            UpdateFront();
        }
    }
    public void UpdateByBlockChange()
    {
        Debug.Log(column.posX + " " + column.posZ);
        meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Block block = blocks[x, y, z];
                    block.SetMeshUp(this, x, y, z, meshData);
                    block.SetMeshDown(this, x, y, z, meshData);
                    block.SetMeshFront(this, x, y, z, meshData);
                    block.SetMeshBack(this, x, y, z, meshData);
                    block.SetMeshLeft(this, x, y, z, meshData);
                    block.SetMeshRight(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh();
    }
    public void UpdateUp()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshUp(this, x, y, z, meshData);
                }
            }
        }
        updateComplete[4] = true;
    }
    public void UpdateDown()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z].SetMeshDown(this, x, y, z, meshData);
                }
            }
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

    /// <summary>
    /// 用于在每一帧中渲染网格数据
    /// </summary>
    public void RenderMesh()
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
    /// <summary>
    /// 用于获取对应位置的方块信息
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Block GetBlock(int x, int y, int z)
    {
        Chunk targetChunk = null;
        if (x < 0)
        {
            if ((targetChunk = this.LeftChunk) != null)
                return targetChunk.GetBlock(x + 16, y, z);
        }
        else if (x > 15)
        {
            if ((targetChunk = this.RightChunk) != null)
                return targetChunk.GetBlock(x - 16, y, z);
        }
        else if (z < 0)
        {
            if ((targetChunk = this.FrontChunk) != null)
                return targetChunk.GetBlock(x, y, z + 16);
        }
        else if (z > 15)
        {
            if ((targetChunk = this.BackChunk) != null)
                return targetChunk.GetBlock(x, y, z - 16);
        }
        else if (y < 0)
        {
            if ((targetChunk = this.DownChunk) != null)
                return targetChunk.GetBlock(x, y + 16, z);
        }
        else if (y > 15)
        {
            if ((targetChunk = this.UpChunk) != null)
                return targetChunk.GetBlock(x, y - 16, z);
        }
        else
        {
            return (chunkSize != 0) ? this[x, y, z] : BlockData.AIR;
        } 
        return BlockData.AIR;
    }
}
