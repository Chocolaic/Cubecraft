using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class LoginSuccessPacket : CubePacket
    {
        public override void Read(InputBuffer input)
        {
            
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
