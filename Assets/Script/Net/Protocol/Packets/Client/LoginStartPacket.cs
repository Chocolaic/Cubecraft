using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class LoginStartPacket : CubePacket
    {
        public string UserName { get; private set; }
        public LoginStartPacket(string username)
        {
            this.UserName = username;
        }
        public override void Read(InputBuffer input)
        {
            this.UserName = input.ReadString();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteString(UserName);
        }
    }
}
