using Cubecraft.Data.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{
    public abstract int BlockID { get; }
    public abstract bool Transparent { get; }
    public enum Direction
    {
        Back,
        Left,
        Front,
        Right,
        Up,
        Down
    };
    const float tileSize = 0.25f;
    public struct Tile { public int x; public int y; }
    public virtual Tile TexturePosition (Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }
    public virtual void SetMeshVertical(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (Block.IsTransparent(chunk.GetBlock(x, y + 1, z)))
        {
            FaceDataUp(x, y, z, meshData);
        }

        if (Block.IsTransparent(chunk.GetBlock(x, y - 1, z)))
        {
            FaceDataDown(x, y, z, meshData);
        }
    }

    public virtual void SetMeshLeft(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (Block.IsTransparent(chunk.GetBlock(x - 1, y, z)))
        {
            FaceDataLeft(x, y, z, meshData);
        }
    }
    public virtual void SetMeshRight(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (Block.IsTransparent(chunk.GetBlock(x + 1, y, z)))
        {
            FaceDataRight(x, y, z, meshData);
        }
    }
    public virtual void SetMeshFront(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (Block.IsTransparent(chunk.GetBlock(x, y, z - 1)))
        {
            FaceDataBack(x, y, z, meshData);
        }
    }
    public virtual void SetMeshBack(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (Block.IsTransparent(chunk.GetBlock(x, y, z + 1)))
        {
            FaceDataFront(x, y, z, meshData);
        }
    }
    public static Vector2[] FaceUVs(Direction direction, Block block)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = block.TexturePosition(direction);
        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y);
        return UVs;
    }
    public static bool IsTransparent(BlockState state)
    {
        return Global.blockDic.GetBlock(state.ID).Transparent;
    }

    protected virtual MeshData FaceDataUp(int x, int y, int z, MeshData meshData)
    {
        //meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f)); //原来的方式
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));      //转换的方式
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        //其它方向上的函数也要添加下面这行代码。
        meshData.uv.AddRange(FaceUVs(Direction.Up, this));
        return meshData;
    }

    protected virtual MeshData FaceDataDown(int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        //Add the following line to every FaceData function with the direction of the face
        meshData.uv.AddRange(FaceUVs(Direction.Down, this));
        return meshData;
    }

    protected virtual MeshData FaceDataFront(int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        //Add the following line to every FaceData function with the direction of the face
        meshData.uv.AddRange(FaceUVs(Direction.Front, this));
        return meshData;
    }

    protected virtual MeshData FaceDataRight(int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        //Add the following line to every FaceData function with the direction of the face
        meshData.uv.AddRange(FaceUVs(Direction.Right, this));
        return meshData;
    }

    protected virtual MeshData FaceDataBack(int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        //Add the following line to every FaceData function with the direction of the face
        meshData.uv.AddRange(FaceUVs(Direction.Back, this));
        return meshData;
    }

    protected virtual MeshData FaceDataLeft(int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        //Add the following line to every FaceData function with the direction of the face
        meshData.uv.AddRange(FaceUVs(Direction.Left, this));
        return meshData;
    }
}
