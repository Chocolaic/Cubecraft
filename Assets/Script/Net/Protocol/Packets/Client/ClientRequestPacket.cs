using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{ 
    class ClientRequestPacket : CubePacket
    {
        public ClientRequest Request { get; private set; }
        public ClientRequestPacket(ClientRequest request)
        {
            this.Request = request;
        }
        public override void Read(InputBuffer input)
        {
            throw new NotImplementedException();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteVarInt((int)Request);
        }
    }
}
