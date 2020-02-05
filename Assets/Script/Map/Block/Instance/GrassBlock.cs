using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cubecraft.Data.World.Blocks
{
    public class GrassBlock : Block
    {
        public override int BlockID { get { return 2; } }
        public override bool Transparent { get { return false; } }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            switch (direction)
            {
                case Direction.Up:
                    tile.x = 2;
                    tile.y = 0;
                    return tile;
                case Direction.Down:
                    tile.x = 1;
                    tile.y = 0;
                    return tile;
            }
            tile.x = 3;
            tile.y = 0;
            return tile;
        }
    }
}
