using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerJoinGamePacket : CubePacket
    {
        public int EntityID { get; private set; }
        public bool HardCore { get; private set; }
        public int GameMode { get; private set; }
        public int Dimension { get; private set; }
        public int Difficulty { get; private set; }
        public override void Read(InputBuffer input)
        {
            this.EntityID = input.ReadInt();
            int gamemode = input.ReadByte();
            this.HardCore = (gamemode & 8) == 8;
            this.GameMode &= -9;
            this.Dimension = input.ReadInt();
            this.Difficulty = input.ReadByte();
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
