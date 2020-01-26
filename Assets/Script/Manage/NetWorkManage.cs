using Cubecraft.Net.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkManage : MonoBehaviour, INetworkHandler
{
    CubeProtocol protocolHander;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        StartWorking(Global.currentServerHost, Global.currentServerPort, Global.protocolVersion);
    }

    public void StartWorking(string host, int port, int protocol)
    {
        protocolHander = new CubeProtocol(host, port, protocol, this);
        protocolHander.LoginToServer(Global.sessionToken);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log(msg);
        gameManager.ShowMsg(alert + msg);
    }
}
