using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerPlayerHealthPacket : CubePacket
    {
        public float Health { get; private set; }
        public int Food { get; private set; }
        public float Saturation { get; private set; }
        public override void Read(InputBuffer input)
        {
            this.Health = input.ReadFloat();
            this.Food = input.ReadVarInt();
            this.Saturation = input.ReadFloat();
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
