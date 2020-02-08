using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ClientPlayerMovementPacket : CubePacket
    {
        public double x { get; protected set; }
        public double y { get; protected set; }
        public double z { get; protected set; }
        public float Yaw { get; protected set; }
        public float Pitch { get; protected set; }
        public bool OnGround { get; protected set; }
        protected bool pos = false;
        protected bool rot = false;
        public ClientPlayerMovementPacket(bool onGround)
        {
            this.OnGround = onGround;
        }
        public override void Read(InputBuffer input)
        {
            throw new NotImplementedException();
        }

        public override void Write(OutputBuffer output)
        {
            if (pos)
            {
                output.WriteDouble(x);
                output.WriteDouble(y);
                output.WriteDouble(z);
            }
            if (rot)
            {
                output.WriteFloat(Yaw);
                output.WriteFloat(Pitch);
            }
            output.WriteBool(OnGround);
        }
    }
}
