using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerChunkDataPacket : CubePacket
    {
        public ChunkColumn Column { get; private set; }
        public override void Read(InputBuffer input)
        {
            int x = input.ReadInt();
            int z = input.ReadInt();
            bool chunksContinuous = input.ReadBool();
            int chunkMask = input.ReadVarInt();
            byte[] data = input.ReadData(input.ReadVarInt());

            this.Column = NetUtils.ReadColumnData(data, x, z, chunksContinuous, false, chunkMask);
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
