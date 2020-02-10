using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World.Blocks
{
    class FlowerBlock : PlantBlock
    {
        public override int BlockID { get { return id; } }
        private int id;
        private Tile tile;
        public FlowerBlock(int id, int textureX, int textureY)
        {
            this.id = id;
            this.tile = new Tile() { x = textureX, y = textureY };
        }
        public override Tile TexturePosition(Direction direction)
        {
            return tile;
        }
    }
}
