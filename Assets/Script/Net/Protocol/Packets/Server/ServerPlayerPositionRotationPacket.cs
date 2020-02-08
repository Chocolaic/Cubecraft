using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cubecraft.Net.Protocol.IO;
using Cubecraft.Net.Protocol.Player;

namespace Cubecraft.Net.Protocol.Packets
{
    class ServerPlayerPositionRotationPacket : CubePacket
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double z { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public List<PositionField> Relative { get; private set; }
        public int TeleportID { get; private set; }
        public override void Read(InputBuffer input)
        {
            this.x = input.ReadDouble();
            this.y = input.ReadDouble();
            this.z = input.ReadDouble();
            this.Yaw = input.ReadFloat();
            this.Pitch = input.ReadFloat();
            int flags = input.ReadByte();
            this.Relative = new List<PositionField>();
            foreach (PositionField element in Enum.GetValues(typeof(PositionField)))
            {
                int bit = 1 << (int)element;
                if ((flags & bit) == bit)
                    this.Relative.Add(element);
            }
            this.TeleportID = input.ReadVarInt();
        }

        public override void Write(OutputBuffer output)
        {
            throw new NotImplementedException();
        }
    }
}
