using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class ClientPlayerPositionPacket : ClientPlayerMovementPacket
    {
        public ClientPlayerPositionPacket(bool onGround, double x, double y, double z) : base(onGround)
        {
            this.pos = true;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
