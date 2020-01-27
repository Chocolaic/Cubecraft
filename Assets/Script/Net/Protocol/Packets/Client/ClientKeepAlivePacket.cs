using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ClientKeepAlivePacket : CubePacket
    {
        public long PingID { get; private set; }
        public ClientKeepAlivePacket(long id)
        {
            this.PingID = id;
        }
        public override void Read(InputBuffer input)
        {
            this.PingID = input.ReadLong();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteLong(PingID);
        }
    }
}
