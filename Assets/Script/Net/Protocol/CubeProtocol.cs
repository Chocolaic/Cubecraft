using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubecraft.Net.Protocol
{
    public delegate void StatusCallBack(string result);
    class CubeProtocol : DataTypes
    {
        internal const int MC18Version = 47;
        internal const int MC19Version = 107;
        internal const int MC191Version = 108;
        internal const int MC110Version = 210;
        internal const int MC1112Version = 316;
        internal const int MC112Version = 335;
        internal const int MC1121Version = 338;
        internal const int MC1122Version = 340;
        internal const int MC113Version = 393;
        internal const int MC114Version = 477;
        internal const int MC1142Version = 485;

        public CubeProtocol(int protocol) : base(protocol) { }
        public async void GetServerInfo(string host, int port, StatusCallBack call)
        {
            await Task.Run(() =>
            {
                try
                {
                    SocketWrapper socketWrapper = new SocketWrapper(host, port);
                    byte[] packet_id = GetVarInt(0),
                        protocol_version = GetVarInt(-1),
                        server_port = BitConverter.GetBytes((ushort)port),
                        next_state = GetVarInt(1);
                    Array.Reverse(server_port);
                    byte[] packet = ConcatBytes(packet_id, protocol_version, GetString(host), server_port, next_state);
                    byte[] tosend = ConcatBytes(GetVarInt(packet.Length), packet);
                    socketWrapper.SendDataRAW(tosend);

                    byte[] status_request = GetVarInt(0),
                        request_packet = ConcatBytes(GetVarInt(status_request.Length), status_request);
                    socketWrapper.SendDataRAW(request_packet);

                    int packetLength = ReadNextVarIntRAW(socketWrapper);
                    if (packetLength > 0)
                    {
                        List<byte> packetData = new List<byte>(socketWrapper.ReadDataRAW(packetLength));
                        if (ReadNextVarInt(packetData) == 0x00)
                        {
                            string result = ReadNextString(packetData);

                            if (!String.IsNullOrEmpty(result) && result.StartsWith("{") && result.EndsWith("}"))
                            {
                                call(result);
                                return;
                            }
                        }
                    }
                }catch(System.Net.Sockets.SocketException e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                }
                call(null);
            });
        }
    }
}
