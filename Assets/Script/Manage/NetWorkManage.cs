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
    bool isWorking;
    // Start is called before the first frame update
    void Start()
    {
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
            }
        }
    }
    public void ChatInput(string text)
    {
        Debug.Log("Input:" + text);
        gameManager.ChatInputCompleted(text);
        protocolHander.SendPacket(new ClientChatPacket(text));
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
