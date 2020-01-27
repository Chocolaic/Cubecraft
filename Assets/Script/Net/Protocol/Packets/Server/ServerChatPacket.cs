using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;
using Cubecraft.Utilities;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerChatPacket : CubePacket
    {
        public string SourceText { get; private set; }
        public string RichText { get; private set; }
        public int Type { get; private set; }
        public override void Read(InputBuffer input)
        {
            this.SourceText = input.ReadString();
            this.Type = input.ReadByte();
            this.RichText = ChatParser.ParseText(SourceText);
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
