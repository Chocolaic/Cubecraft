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
        public static Block AIR { get; private set; }

        public void RegisterAll()
        {
            RegisterBlock(0, new AirBlock());
            RegisterBlock(1, new StoneBlock());
            RegisterBlock(2, new GrassBlock());
            RegisterBlock(3, new DirtBlock());
            RegisterBlock(5, new PlanksBlock());
            RegisterBlock(9, new WaterBlock());
            RegisterBlock(12, new SandBlock());
            RegisterBlock(13, new GravelBlock());
            RegisterBlock(17, new WoodBlock());
            RegisterBlock(18, new LeavesBlock());
            RegisterBlock(31, new PlantBlock());
            RegisterBlock(37, new FlowerBlock(37, 7, 0));
            AIR = blockDic[0];
        }
    }
}
