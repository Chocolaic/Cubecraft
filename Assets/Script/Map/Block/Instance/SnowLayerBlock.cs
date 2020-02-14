using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cubecraft.Data.World.Blocks
{
    class SnowLayerBlock : Block
    {
        public override int BlockID { get { return 78; } }

        public override bool Transparent { get { return true; } }
        protected override MeshData FaceDataUp(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.3f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.3f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.3f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.3f, z - 0.5f));

            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.Up, this));
            return meshData;
        }

        protected override MeshData FaceDataBack(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.3f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.3f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Front, this));
            return meshData;
        }

        protected override MeshData FaceDataRight(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.3f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.3f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Right, this));
            return meshData;
        }

        protected override MeshData FaceDataFront(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.3f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.3f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Back, this));
            return meshData;
        }

        protected override MeshData FaceDataLeft(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.3f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.3f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();
            //Add the following line to every FaceData function with the direction of the face
            meshData.uv.AddRange(FaceUVs(Direction.Left, this));
            return meshData;
        }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 5;
            tile.y = 1;
            return tile;
        }
    }
}
