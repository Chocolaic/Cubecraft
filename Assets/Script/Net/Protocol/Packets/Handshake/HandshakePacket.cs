using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubecraft.Net.Protocol.IO;

namespace Cubecraft.Net.Protocol.Packets
{
    class HandshakePacket : CubePacket
    {
        public int ProtocolVersion { get; private set; }
        public string HostName { get; private set; }
        public short Port { get; private set; }
        public int Intent { get; private set; }
        public HandshakePacket(int protocol, string hostname, short port, int intent)
        {
            this.ProtocolVersion = protocol;
            this.HostName = hostname;
            this.Port = port;
            this.Intent = intent;
        }
        public override void Read(InputBuffer input)
        {
            throw new NotImplementedException();
        }

        public override void Write(OutputBuffer output)
        {
            output.WriteVarInt(ProtocolVersion);
            output.WriteString(HostName);
            output.WriteShort(Port);
            output.WriteVarInt(Intent);
        }
    }
}
