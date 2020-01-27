using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ClientChatPacket : CubePacket
    {
        public string Text { get; private set; }
        public ClientChatPacket(string text)
        {
            this.Text = text;
        }
        public override void Read(InputBuffer input)
        {
            throw new NotImplementedException();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteString(Text);
        }
    }
}
