using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World.Blocks
{
    class WoodBlock : Block
    {
        public override int BlockID { get { return 17; } }

        public override bool Transparent { get { return false; } }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            switch (direction)
            {
                case Direction.Up:
                case Direction.Down:
                    tile.x = 2;
                    tile.y = 1;
                    return tile;
            }
            tile.x = 3;
            tile.y = 1;
            return tile;
        }
    }
}
