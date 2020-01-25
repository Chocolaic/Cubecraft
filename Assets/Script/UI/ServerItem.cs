using Cubecraft.Net.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ServerItem : MonoBehaviour
{
    [SerializeField] private GameObject texthost;
    [SerializeField] private GameObject textstatus;
    private Text Host;
    private Text Status;

    public string host;
    public string status = "<color=#3AB3DA>正在连接服务器...</color>";
    public void SetInfo(string host)
    {
        this.host = host;

        DoPing(host);
    }
    private void DoPing(string server)
    {
        string[] sip = server.Replace("：", ":").Split(':');
        string host = sip[0];
        ushort port = 25565;
        if (sip.Length > 1)
            port = ushort.Parse(sip[1]);
        else
            ProtocolHandler.MinecraftServiceLookup(ref host, ref port);

        CubeProtocol protocol = new CubeProtocol(CubeProtocol.MC18Version);
        protocol.GetServerInfo(host, port, (string result) => {
            Debug.Log(result);
            if (result != null)
            {
                status = ColorUtility.Set(ColorUtility.Green, "-ONLINE-");
            }else
                status = ColorUtility.Set(ColorUtility.Red, "-OFFLINE-");
        });
    }
    void Start()
    {
        Host = texthost.GetComponent<Text>();
        Status = textstatus.GetComponent<Text>();

        Host.text = host;
    }
    void Update()
    {
        Status.text = status;
    }
}
