using Cubecraft.Net.Protocol.IO;
using Cubecraft.Net.Templates;
using UnityEngine;

namespace Cubecraft.Net.Protocol.Packets
{
    class StatusResponsePacket : CubePacket
    {
        public StatusInfo Info { get; private set; }
        public override void Read(InputBuffer input)
        {
            string result = input.ReadString();
            Debug.Log(result);
            Info = JsonUtility.FromJson<StatusInfo>(result);
        }

        public override void Write(OutputBuffer output)
        {

        }
    }
}
