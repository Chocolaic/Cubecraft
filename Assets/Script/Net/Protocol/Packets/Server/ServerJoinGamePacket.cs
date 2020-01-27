using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerJoinGamePacket : CubePacket
    {
        public int EntityID { get; private set; }
        public override void Read(InputBuffer input)
        {
            this.EntityID = input.ReadInt();
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
