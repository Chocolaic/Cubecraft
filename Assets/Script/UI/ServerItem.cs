using Cubecraft.Net.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class ServerItem : MonoBehaviour
{
    [SerializeField] private GameObject texthost;
    [SerializeField] private GameObject textstatus;
    private Text Host;
    private Text Status;

    public string host;
    public int protocol;
    public string status = "<color=#3AB3DA>正在连接服务器...</color>";
    public void SetInfo(string host)
    {
        this.host = host;

        DoPing(host);
    }
    private void DoPing(string server)
    {
        ushort port = 25565;
        GetServerAddr(ref server, ref port);
        CubeProtocol.GetServerInfo(server, port, (Cubecraft.Net.Templates.StatusInfo result) => {
            if (result != null)
            {
                status = ColorUtility.Set(ColorUtility.Green, "-ONLINE-");
            }else
                status = ColorUtility.Set(ColorUtility.Red, "-OFFLINE-");
        });
    }
    public void EnterServer()
    {
        string server = this.host;
        ushort port = 25565;
        GetServerAddr(ref server, ref port);
        Global.currentServerHost = server;
        Global.currentServerPort = port;
        Global.protocolVersion = protocol;
        SceneManager.LoadSceneAsync("MapInstance");
    }
    private void GetServerAddr(ref string server, ref ushort port)
    {
        string[] sip = server.Replace("：", ":").Split(':');
        server = sip[0];
        port = 25565;
        if (sip.Length > 1)
            port = ushort.Parse(sip[1]);
        else
            ProtocolHandler.MinecraftServiceLookup(ref server, ref port);
    }
    void Start()
    {
        Host = texthost.GetComponent<Text>();
        Status = textstatus.GetComponent<Text>();

        Host.text = host;
        protocol = Protocol.MC1122Version;
    }
    void Update()
    {
        Status.text = status;
    }
}
