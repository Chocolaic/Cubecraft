using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerRespawnPacket : CubePacket
    {
        public int Dimension { get; private set; }
        public int Difficulty { get; private set; }
        public int GameMode { get; private set; }

        public override void Read(InputBuffer input)
        {
            this.Dimension = input.ReadInt();
            this.Difficulty = input.ReadByte();
            this.GameMode = input.ReadByte();
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
