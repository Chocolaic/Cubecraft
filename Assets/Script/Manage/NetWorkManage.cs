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
    ConnectionStatus connection;
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
        try
        {
            protocolHander = new CubeProtocol(host, port, protocol, this);
        }
        catch(System.Exception e)
        {
            OnConnectionLost(DisconnectReason.ConnectionLost, ColorUtility.Set(ColorUtility.Red, e.Message));
            return;
        }
        protocolHander.LoginToServer(Global.sessionToken);
    }

    // Update is called once per frame
    void Update()
    {
        if(isWorking)
            HandlePacket();
        else if (connection.connectionLostTrigger)
            ConnectionLostHandle();
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
            else if (packet.GetType() == typeof(ServerJoinGamePacket))
            {
                ServerJoinGamePacket joinGamePacket = (ServerJoinGamePacket)packet;
                this.entityid = joinGamePacket.EntityID;
                CubeProtocol.currentDimension = joinGamePacket.Dimension;
            }else if (packet.GetType() == typeof(ServerChunkDataPacket))
            {
                mapManager.chunkQueue.Enqueue(((ServerChunkDataPacket)packet).Column);
            }
            else if (packet.GetType() == typeof(ServerPlayerPositionRotationPacket))
            {
                gameManager.EndLoading();
                ServerPlayerPositionRotationPacket positionAndLook = (ServerPlayerPositionRotationPacket)packet;
                //Debug.Log("X:" + positionAndLook.x + " Y:" + positionAndLook.y + " z:" + positionAndLook.z);
                SendPacket(new ClientTeleportConfirmPacket(positionAndLook.TeleportID));
                List<PositionField> posfield = positionAndLook.Relative;
                mapManager.SetPlayerPosition(new Vector3(
                    (float)(posfield.Contains(PositionField.X) ? 0 : positionAndLook.x),
                    (float)(posfield.Contains(PositionField.Y) ? 0 : positionAndLook.y)+1,
                    (float)(posfield.Contains(PositionField.Z) ? 0 : positionAndLook.z)));
            }else if (packet.GetType() == typeof(ServerPlayerHealthPacket))
            {
                if (((ServerPlayerHealthPacket)packet).Health == 0)
                    SendPacket(new ClientRequestPacket(ClientRequest.Respawn));
            }else if (packet.GetType() == typeof(ServerRespawnPacket))
            {
                Debug.Log("Clear Dimension");
                mapManager.UnloadDimension();
            }
            else if (packet.GetType() == typeof(ServerUnloadChunkPacket))
            {
                ServerUnloadChunkPacket unloadChunkPacket = (ServerUnloadChunkPacket)packet;
                mapManager.GetWorld().DestroyColumn(unloadChunkPacket.x, unloadChunkPacket.z);
            }
        }
    }
    public void ChatInput(string text)
    {
        Debug.Log("Input:" + text);
        gameManager.ChatInputCompleted(text);
        SendPacket(new ClientChatPacket(text));
    }
    internal void SendPacket(Packet packet)
    {
        protocolHander.GetOutgoingQueue().Add(packet);
    }
    public void OnConnectionLost(DisconnectReason reason, string msg)
    {
        this.isWorking = false;
        Debug.Log("Connection Lost");
        connection.connectionLostTrigger = true;
        connection.reason = reason;
        connection.msg = msg;
    }
    void ConnectionLostHandle()
    {
        string alert = string.Empty;
        switch (connection.reason)
        {
            case DisconnectReason.ConnectionLost: alert = "连接丢失："; break;
            case DisconnectReason.InGameKick: alert = "您被请出："; break;
            case DisconnectReason.LoginRejected: alert = "拒绝登录："; break;
        }
        Disconnect();
        gameManager.InterruptGame(alert + connection.msg);
    }

    public void OnGameJoined()
    {
        Debug.Log("LoginSuccess");
    }
    public void Disconnect()
    {
        if(protocolHander != null)
            protocolHander.Dispose();
    }
}
