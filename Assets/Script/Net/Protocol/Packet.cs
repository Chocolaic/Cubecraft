using Cubecraft.Net.Protocol.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Net.Protocol
{
    interface Packet
    {
        void Read(InputBuffer input);
        void Write(OutputBuffer output);
    }
}
