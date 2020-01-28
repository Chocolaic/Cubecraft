using Cubecraft.Net.Protocol;
using Cubecraft.Net.Protocol.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkManage : MonoBehaviour, INetworkHandler
{
    CubeProtocol protocolHander;
    GameManager gameManager;
    MapManager mapManager;
    bool isWorking;

    int entityid;

    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("Manager").GetComponent<MapLoader>();
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
        while (protocolHander.GetIncomingQueue().TryTake(out packet))
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
                this.entityid = ((ServerJoinGamePacket)packet).EntityID;
            }else if(packet.GetType() == typeof(ServerChunkDataPacket))
            {
                SetChunkData(((ServerChunkDataPacket)packet).Column);
            }
        }
    }
    private void SetChunkData(ChunkColumn column)
    {
        for(int y = 0; y < 256; y+=16)
        {
            ChunkData chunk = column[y];
            for(int blockY = 0; blockY < ChunkData.SizeY; blockY++)
            {
                for (int blockX = 0; blockX < 16; blockX++)
                {
                    for (int blockZ = 0; blockZ < 16; blockZ++)
                    {
                        AbstractBlock block = (AbstractBlock)chunk[blockX, blockY, blockZ];
                        mapManager.SetBlock(block.BlockID, new Vector3(column.ChunkX + blockX, y + blockY, column.ChunkZ + blockZ));
                    }
                }
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
        gameManager.ShowMsg(alert + msg);
    }

    public void OnGameJoined()
    {
        Debug.Log("LoginSuccess");
    }
}
