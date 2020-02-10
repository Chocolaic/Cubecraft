using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World.Blocks
{
    class PlanksBlock : Block
    {
        public override int BlockID { get { return 5; } }

        public override bool Transparent { get { return false; } }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 4;
            tile.y = 1;
            return tile;
        }
    }
}
