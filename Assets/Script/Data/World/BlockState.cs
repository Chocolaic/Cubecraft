using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Data.World
{
    public struct BlockState
    {
        public int ID { get; private set; }
        public int Data { get; private set; }
        public BlockState(int id, int data)
        {
            this.ID = id;
            this.Data = data;
        }
    }
}
