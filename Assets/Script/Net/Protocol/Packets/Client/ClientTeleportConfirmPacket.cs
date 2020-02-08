using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ClientTeleportConfirmPacket : CubePacket
    {
        public int TeleportID { get; private set; }
        public ClientTeleportConfirmPacket(int id)
        {
            this.TeleportID = id;
        }
        public override void Read(InputBuffer input)
        {
            throw new NotImplementedException();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteVarInt(TeleportID);
        }
    }
}
