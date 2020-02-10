using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cubecraft.Data.World.Blocks
{
    class PlantBlock : Block
    {
        public override int BlockID { get { return 6; } }

        public override bool Transparent { get { return true; } }
        public override void SetMeshVertical(Chunk chunk, int x, int y, int z, MeshData meshData) { }
        public override void SetMeshLeft(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            if (Block.IsTransparent(chunk.GetBlock(x - 1, y, z)))
            {
                FaceDataLeft(x, y, z, meshData);
            }
        }
        public override void SetMeshRight(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            if (Block.IsTransparent(chunk.GetBlock(x + 1, y, z)))
            {
                FaceDataRight(x, y, z, meshData);
            }
        }
        public override void SetMeshFront(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            if (Block.IsTransparent(chunk.GetBlock(x, y, z - 1)))
            {
                FaceDataBack(x, y, z, meshData);
            }
        }
        public override void SetMeshBack(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            if (Block.IsTransparent(chunk.GetBlock(x, y, z + 1)))
            {
                FaceDataFront(x, y, z, meshData);
            }
        }
        protected override MeshData FaceDataFront(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Front, this));
            return meshData;
        }

        protected override MeshData FaceDataRight(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Right, this));
            return meshData;
        }

        protected override MeshData FaceDataBack(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Back, this));
            return meshData;
        }

        protected override MeshData FaceDataLeft(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Left, this));
            return meshData;
        }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 6;
            tile.y = 0;
            return tile;
        }
    }
}
