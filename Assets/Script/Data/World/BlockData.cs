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
        private Type[] blockDic = new Type[256];
        public void RegisterBlock<T>(int id) where T : Block
        {
            blockDic[id] = typeof(T);
        }
        public Block Instantiate(int id)
        {
            Type block = null;
            if ((block = blockDic[id]) != null)
                return (Block)Activator.CreateInstance(block);
            else
                return new GrassBlock();
        }

        public void RegisterAll()
        {
            RegisterBlock<AirBlock>(0);
            RegisterBlock<StoneBlock>(1);
            RegisterBlock<GrassBlock>(2);
            RegisterBlock<DirtBlock>(3);
            RegisterBlock<WoodBlock>(17 );
        }
    }
}
