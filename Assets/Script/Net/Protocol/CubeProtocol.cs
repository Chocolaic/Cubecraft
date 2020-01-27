using Cubecraft.Net.Protocol.Packets;
using Cubecraft.Net.Templates;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

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
                case SubProtocol.Game:
                    initGame(); break;
            }
        }
        #region
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
        private void initGame()
        {
            RegisterOutgoing<ClientChatPacket>(0x02);
            RegisterOutgoing<ClientKeepAlivePacket>(0x0B);

            RegisterIncoming<ServerChatPacket>(0x0F);
            RegisterIncoming<ServerDisconnectPacket>(0x1A);
            RegisterIncoming<ServerKeepAlivePacket>(0x1F);
            RegisterIncoming<ServerJoinGamePacket>(0x23);
        }
        #endregion
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
                        UnityEngine.Debug.Log(((LoginDisconnectPacket)packet).SourceText);
                        handler.OnConnectionLost(DisconnectReason.LoginRejected, ((LoginDisconnectPacket)packet).RichText);
                        return;
                    }else if(packet.GetType() == typeof(LoginSuccessPacket))
                    {
                        handler.OnGameJoined();
                        SetProtocol(SubProtocol.Game);
                        break;
                    }else if(packet.GetType() == typeof(LoginSetCompressionPacket))
                    {
                        SetCompress(((LoginSetCompressionPacket)packet).Threshold);
                    }
                }
                else
                    handler.OnConnectionLost(DisconnectReason.ConnectionLost, ColorUtility.Set(ColorUtility.Red, "与服务器断开连接"));
            }

            netRead = new System.Threading.Thread(() => {
                while (socketWrapper.IsConnected())
                {
                    Packet packet = ReadPacket();
                    if (packet != null)
                    {
                        if (packet.GetType() == typeof(ServerKeepAlivePacket))
                        {
                            SendPacket(new ClientKeepAlivePacket(((ServerKeepAlivePacket)packet).PingID));
                        }
                        else if (packet.GetType() == typeof(ServerDisconnectPacket))
                            return;
                        else
                            incomingQueue.Add(packet);
                    }
                }
                Debug.Log(netRead.Name + " stoped.");
            });
            netRead.Name = "PacketHandler";
            netRead.Start();
        }

        public static async void GetServerInfo(string host, int port, StatusCallBack call)
        {
            await Task.Run(() =>
            {
                CubeProtocol protocol = null;
                try
                {
                    protocol = new CubeProtocol(host, port, MC1122Version, null);
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
        public BlockingCollection<Packet> GetIncomingQueue()
        {
            return this.incomingQueue;
        }
        public BlockingCollection<Packet> GetOutgoingQueue()
        {
            return this.outgoingQueue;
        }
    }
}
