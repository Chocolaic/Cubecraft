using Cubecraft.Data.World;
using Cubecraft.Net.Protocol;
using Cubecraft.Net.Protocol.Packets;
using Cubecraft.Net.Protocol.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkManage : MonoBehaviour, INetworkHandler
{
    CubeProtocol protocolHander;
    MapManager mapManager;
    GameManager gameManager;
    bool isWorking;

    int entityid;

    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("Manager").GetComponent<MapManager>();
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        gameManager.chatInput.GetComponent<InputField>().onEndEdit.AddListener(ChatInput);
        StartWorking(Global.currentServerHost, Global.currentServerPort, Global.protocolVersion);
    }

    public void StartWorking(string host, int port, int protocol)
    {
        isWorking = true;
        protocolHander = new CubeProtocol(host, port, protocol, this);
        protocolHander.LoginToServer(Global.sessionToken);
    }

    // Update is called once per frame
    void Update()
    {
        if(isWorking)
            HandlePacket();
    }
    void HandlePacket()
    {
        Packet packet = null;
        if (protocolHander.GetIncomingQueue().TryTake(out packet))
        {
            if (packet.GetType() == typeof(ServerChatPacket))
            {
                gameManager.AddChatText(((ServerChatPacket)packet).RichText);
            }
            else if (packet.GetType() == typeof(ServerDisconnectPacket))
            {
                OnConnectionLost(DisconnectReason.InGameKick, ((ServerDisconnectPacket)packet).RichText);
                return;
            }else if(packet.GetType() == typeof(ServerJoinGamePacket))
            {
                ServerJoinGamePacket joinGamePacket = (ServerJoinGamePacket)packet;
                this.entityid = joinGamePacket.EntityID;
                CubeProtocol.currentDimension = joinGamePacket.Dimension;
            }else if(packet.GetType() == typeof(ServerChunkDataPacket))
            {
                mapManager.chunkQueue.Enqueue(((ServerChunkDataPacket)packet).Column);
            }
            else if(packet.GetType() == typeof(ServerPlayerPositionRotationPacket))
            {
                gameManager.EndLoading();
                ServerPlayerPositionRotationPacket positionAndLook = (ServerPlayerPositionRotationPacket)packet;
                Debug.Log("X:" + positionAndLook.x + " Y:" + positionAndLook.y + " z:" + positionAndLook.z);
                List<PositionField> posfield = positionAndLook.Relative;
                mapManager.SetPlayerPosition(new Vector3(
                    (float)(posfield.Contains(PositionField.X) ? 0 : positionAndLook.x),
                    (float)(posfield.Contains(PositionField.Y) ? 0 : positionAndLook.y) + 20f,
                    (float)(posfield.Contains(PositionField.Z) ? 0 : positionAndLook.z)));
            }
        }
    }
    public void ChatInput(string text)
    {
        Debug.Log("Input:" + text);
        gameManager.ChatInputCompleted(text);
        SendPacket(new ClientChatPacket(text));
    }
    private void SendPacket(Packet packet)
    {
        protocolHander.GetOutgoingQueue().Add(packet);
    }

    public void OnConnectionLost(DisconnectReason reason, string msg)
    {
        string alert = string.Empty;
        switch (reason)
        {
            case DisconnectReason.ConnectionLost: alert = "连接丢失："; break;
            case DisconnectReason.InGameKick: alert = "您被请出："; break;
            case DisconnectReason.LoginRejected: alert = "拒绝登录："; break;
        }
        protocolHander.Dispose();
        gameManager.InterruptGame(alert + msg);
    }

    public void OnGameJoined()
    {
        Debug.Log("LoginSuccess");
    }
}
