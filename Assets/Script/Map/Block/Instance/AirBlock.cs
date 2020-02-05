using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World.Blocks
{
    public class AirBlock : Block
    {
        public override int BlockID { get { return 0; } }

        public override bool Transparent { get { return true; } }
        public override void SetMeshVertical(Chunk chunk, int x, int y, int z, MeshData meshData) { }
        public override void SetMeshLeft(Chunk chunk, int x, int y, int z, MeshData meshData) { }
        public override void SetMeshRight(Chunk chunk, int x, int y, int z, MeshData meshData) { }
        public override void SetMeshFront(Chunk chunk, int x, int y, int z, MeshData meshData) { }
        public override void SetMeshBack(Chunk chunk, int x, int y, int z, MeshData meshData) { }
    }
}
