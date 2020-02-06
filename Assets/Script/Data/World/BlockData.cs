using Cubecraft.Data.World.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World
{
    class BlockData
    {
        //提高读取速度而没有用字典
        private Block[] blockDic = new Block[256];
        public void RegisterBlock(int id, Block block)
        {
            blockDic[id] = block;
        }
        public Block GetBlock(int id)
        {
            Block block = null;
            if ((block = blockDic[id]) != null)
                return block;
            else
                return new GrassBlock();
        }

        public void RegisterAll()
        {
            RegisterBlock(0, new AirBlock());
            RegisterBlock(1, new StoneBlock());
            RegisterBlock(2, new GrassBlock());
            RegisterBlock(3, new DirtBlock());
            RegisterBlock(17, new WoodBlock());
        }
    }
}
