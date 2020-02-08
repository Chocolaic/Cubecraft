using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerUnloadChunkPacket : CubePacket
    {
        public int x { get; private set; }
        public int z { get; private set; }
        public override void Read(InputBuffer input)
        {
            this.x = input.ReadInt();
            this.z = input.ReadInt();
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
