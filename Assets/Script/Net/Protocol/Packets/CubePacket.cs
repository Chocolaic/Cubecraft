using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    abstract class CubePacket : Packet
    {
        public abstract void Read(InputBuffer input);

        public abstract void Write(OutputBuffer output);
    }
}
