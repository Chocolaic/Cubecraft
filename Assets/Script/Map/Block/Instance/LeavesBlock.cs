using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World.Blocks
{
    class LeavesBlock : Block
    {
        public override int BlockID { get { return 18; } }

        public override bool Transparent { get { return true; } }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 0;
            tile.y = 1;
            return tile;
        }
    }
}
