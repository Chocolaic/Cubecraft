using Cubecraft.Data.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Chunk : MonoBehaviour
{

    private Block[,,] blocks = new Block[16, 16, 16];
    public const int chunkSize = 16;
    public MeshCollider coll, objcoll;
    private bool onload = false;  //一个标志位，用于判断该数据块是否已经更新

    public MeshFilter blockfilter, objectfilter;
    public MeshData blockMeshData = new MeshData(), objectMeshData = new MeshData();
    public Column column;
    public int position;
    public int Size { get; private set; }
    public Chunk FrontChunk { get; set; }
    public Chunk BackChunk { get; set; }
    public Chunk LeftChunk { get; set; }
    public Chunk RightChunk { get; set; }
    public Chunk UpChunk { get; set; }
    public Chunk DownChunk { get; set; }
    private bool[] aroundComplete = new bool[6];

    void Awake()
    {
        Transform a = transform.GetChild(0);
        Transform b = transform.GetChild(1);
        blockfilter = b.GetComponent<MeshFilter>();
        objectfilter = a.GetComponent<MeshFilter>();
        coll = b.GetComponent<MeshCollider>();
        objcoll = a.GetComponent<MeshCollider>();
    }
    void LateUpdate()
    {
        if (aroundComplete[0] && aroundComplete[1] && aroundComplete[2] && aroundComplete[3] && aroundComplete[4] && aroundComplete[5])
        {
            aroundComplete[0] = false;
            aroundComplete[1] = false;
            aroundComplete[2] = false;
            aroundComplete[3] = false;
            aroundComplete[4] = false;
            aroundComplete[5] = false;
            RenderMesh();
        }
    }
    private void OnDestroy()
    {
        Chunk targetChunk = null;
        if ((targetChunk = this.LeftChunk) != null)
            targetChunk.RightChunk = null;
        if ((targetChunk = this.RightChunk) != null)
            targetChunk.LeftChunk = null;
        if ((targetChunk = this.FrontChunk) != null)
            targetChunk.BackChunk = null;
        if ((targetChunk = this.BackChunk) != null)
            targetChunk.FrontChunk = null;
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
        this.Size++;
    }

    /// <summary>
    /// 用于在每一帧中更新网格数据
    /// </summary>
    public void UpdateChunk()
    {
        blockMeshData = new MeshData();
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
        blockMeshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Block block = blocks[x, y, z];
                    block.SetMeshUp(this, x, y, z);
                    block.SetMeshDown(this, x, y, z);
                    block.SetMeshFront(this, x, y, z);
                    block.SetMeshBack(this, x, y, z);
                    block.SetMeshLeft(this, x, y, z);
                    block.SetMeshRight(this, x, y, z);
                }
            }
        }
        RenderMesh();
    }
    public void UpdateUp()
    {
        if (!aroundComplete[4])
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z].SetMeshUp(this, x, y, z);
                    }
                }
            }
            aroundComplete[4] = true;
        }
    }
    public void UpdateDown()
    {
        if (!aroundComplete[5])
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z].SetMeshDown(this, x, y, z);
                    }
                }
            }
            aroundComplete[5] = true;
        }
    }

    public  void UpdateLeft()
    {
        if (!aroundComplete[0])
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z].SetMeshLeft(this, x, y, z);
                    }
                }
            }
            aroundComplete[0] = true;
        }
    }
    public void UpdateRight()
    {
        if (!aroundComplete[1])
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z].SetMeshRight(this, x, y, z);
                    }
                }
            }
            aroundComplete[1] = true;
        }
    }
    public void UpdateFront()
    {
        if (!aroundComplete[2])
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z].SetMeshFront(this, x, y, z);
                    }
                }
            }
            aroundComplete[2] = true;
        }
    }
    public void UpdateBack()
    {
        if (!aroundComplete[3])
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z].SetMeshBack(this, x, y, z);
                    }
                }
            }
            aroundComplete[3] = true;
        }
    }

    /// <summary>
    /// 用于在每一帧中渲染网格数据
    /// </summary>
    public void RenderMesh()
    {
        blockfilter.mesh.Clear();
        blockfilter.mesh.vertices = blockMeshData.vertices.ToArray();
        blockfilter.mesh.triangles = blockMeshData.triangles.ToArray();

        blockfilter.mesh.uv = blockMeshData.uv.ToArray();
        blockfilter.mesh.RecalculateNormals();

        objectfilter.mesh.Clear();
        objectfilter.mesh.vertices = objectMeshData.vertices.ToArray();
        objectfilter.mesh.triangles = objectMeshData.triangles.ToArray();

        objectfilter.mesh.uv = objectMeshData.uv.ToArray();
        objectfilter.mesh.RecalculateNormals();
        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = blockMeshData.colVertices.ToArray();
        mesh.triangles = blockMeshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;

        objcoll.sharedMesh = null;
        Mesh objmesh = new Mesh();
        objmesh.vertices = objectMeshData.colVertices.ToArray();
        objmesh.triangles = objectMeshData.colTriangles.ToArray();
        objmesh.RecalculateNormals();

        objcoll.sharedMesh = mesh;
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
            return (Size > 0) ? this[x, y, z] : BlockData.AIR;
        } 
        return BlockData.AIR;
    }
}
