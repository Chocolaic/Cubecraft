using Cubecraft.Net.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameBox : MonoBehaviour
{
    [SerializeField]
    public GameObject textusername;
    public GameObject textpassword;
    public GameObject msgBox;

    private ProtocolHandler.LoginResult loginResult = ProtocolHandler.LoginResult.LoginRequired;
    private bool waitForLogin = false;
    public void Login()
    {
        SessionToken session = new SessionToken();
        string username = textusername.GetComponent<InputField>().text;
        string password = textpassword.GetComponent<InputField>().text;
        if (!string.IsNullOrEmpty(username))
        {
            if (string.IsNullOrEmpty(password))
            {
                Debug.Log(username + " OFFLINE");
                session.selectedProfile = new SessionToken.SelectedProfile() { id = "0", name = username };
                Global.sessionToken = session;
                gameObject.SetActive(false);
            }
            else
            {
                ProtocolHandler.GetLogin(username, password, (ProtocolHandler.LoginResult result, SessionToken token) =>
                {
                    Global.sessionToken = token;
                    loginResult = result;
                    waitForLogin = true;
                    if(result == ProtocolHandler.LoginResult.Success)
                        Manager.SaveData("session", token);
                });
            }
        }
    }
    void Update()
    {
        if (waitForLogin)
        {
            msgBox.SetActive(true);
            if (loginResult == ProtocolHandler.LoginResult.Success)
            {
                Debug.Log("Login Success");
                msgBox.GetComponent<Text>().text = ColorUtility.Set(ColorUtility.Green, "欢迎你，" + Global.sessionToken.selectedProfile.name);
                gameObject.SetActive(false);
            }
            else if(loginResult == ProtocolHandler.LoginResult.InvalidResponse)
            {
                msgBox.GetComponent<Text>().text = ColorUtility.Set(ColorUtility.Red, "登录失败，请重试");
            }
            waitForLogin = false;
            Invoke("CloseMsgBox", 3);
        }
    }
    void CloseMsgBox()
    {
        msgBox.GetComponent<Text>().text = "";
        msgBox.SetActive(false);
    }
}
