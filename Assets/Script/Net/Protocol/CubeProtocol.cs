using Cubecraft.Net.Protocol.IO;
using Cubecraft.Net.Protocol.Packets;
using Cubecraft.Net.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cubecraft.Net.Protocol
{
    public delegate void StatusCallBack(StatusInfo result);
    class CubeProtocol : Protocol
    {
        string host;
        int port;
        INetworkHandler handler;
        public CubeProtocol(string host, int port, int protocol, INetworkHandler handler) : base(new SocketWrapper(host, port), protocol)
        {
            this.host = host;
            this.port = port;
            this.handler = handler;
        }

        public void SetProtocol(SubProtocol subProtocol)
        {
            ClearPackets();
            switch (subProtocol)
            {
                case SubProtocol.Handshake:
                    initHandshake(); break;
                case SubProtocol.Status:
                    initStatus(); break;
                case SubProtocol.Login:
                    initLogin(); break;
            }
        }
        private void initHandshake()
        {
            RegisterOutgoing<HandshakePacket>(0x00);
        }
        private void initStatus()
        {
            RegisterOutgoing<StatusQueryPacket>(0x00);

            RegisterIncoming<StatusResponsePacket>(0x00);
        }
        private void initLogin()
        {
            RegisterOutgoing<LoginStartPacket>(0x00);

            RegisterIncoming<LoginDisconnectPacket>(0x00);
            RegisterIncoming<LoginSuccessPacket>(0x02);
            RegisterIncoming<LoginSetCompressionPacket>(0x03);
        }
        public void LoginToServer(SessionToken session)
        {
            SetProtocol(SubProtocol.Handshake);
            SendPacket(new HandshakePacket(ProtocolVersion, host, (short)port, 2));
            SetProtocol(SubProtocol.Login);
            SendPacket(new LoginStartPacket(session.selectedProfile.name));

            while (socketWrapper.IsConnected())
            {
                Packet packet = ReadPacket();
                if (packet != null)
                {
                    if(packet.GetType() == typeof(LoginDisconnectPacket))
                    {
                        UnityEngine.Debug.Log(((LoginDisconnectPacket)packet).RichText);
                        handler.OnConnectionLost(DisconnectReason.LoginRejected, ((LoginDisconnectPacket)packet).RichText);
                        return;
                    }
                }
                else
                    break;
            }
            handler.OnConnectionLost(DisconnectReason.ConnectionLost, ColorUtility.Set(ColorUtility.Red, "与服务器断开连接"));
        }

        public static async void GetServerInfo(string host, int port, StatusCallBack call)
        {
            await Task.Run(() =>
            {
                CubeProtocol protocol = null;
                try
                {
                    protocol = new CubeProtocol(host, port, MC112Version, null);
                    protocol.SetProtocol(SubProtocol.Handshake);
                    protocol.SendPacket(new HandshakePacket(protocol.ProtocolVersion, host, (short)port, 1));
                    protocol.SetProtocol(SubProtocol.Status);
                    protocol.SendPacket(new StatusQueryPacket());
                    var packet = protocol.ReadPacket();
                    if (packet != null && packet.GetType() == typeof(StatusResponsePacket))
                    {
                        call(((StatusResponsePacket)packet).Info);
                    }
                    protocol.Dispose();
                    return;
                }
                catch(System.Net.Sockets.SocketException e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                    if(protocol != null)
                        protocol.Dispose();
                }
                call(null);
            });
        }
    }
}
