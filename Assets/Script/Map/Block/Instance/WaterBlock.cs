using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World.Blocks
{
    class WaterBlock : SlabBlock
    {
        public override int BlockID { get { return 9; } }
        public override void SetMeshVertical(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            base.SetMeshVertical(chunk, x, y, z, meshData);
            meshData.useRenderDataForCol = true;
        }
        public override void SetMeshBack(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            base.SetMeshBack(chunk, x, y, z, meshData);
            meshData.useRenderDataForCol = true;
        }
        public override void SetMeshFront(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            base.SetMeshFront(chunk, x, y, z, meshData);
            meshData.useRenderDataForCol = true;
        }
        public override void SetMeshLeft(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            base.SetMeshLeft(chunk, x, y, z, meshData);
            meshData.useRenderDataForCol = true;
        }
        public override void SetMeshRight(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCol = false;
            base.SetMeshRight(chunk, x, y, z, meshData);
            meshData.useRenderDataForCol = true;
        }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 3;
            tile.y = 2;
            return tile;
        }
    }
}
