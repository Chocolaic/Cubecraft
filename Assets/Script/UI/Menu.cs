using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    public GameObject nameBox;

    public void BtnOffline_Click()
    {
        Debug.Log("OfflineMode");
        Global.state = false;
        SceneManager.LoadSceneAsync("MapInstance");
    }
    public void BtnOnline_Click()
    {
        Debug.Log("OnlineMode");
        Global.state = true;
        SceneManager.LoadSceneAsync("ServerList");
    }

    public void BtnExit_Click()
    {
        Application.Quit();
    }

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/session"))
        {
            Global.sessionToken = Manager.LoadData<SessionToken>("session"); 
        }
        else
            nameBox.SetActive(true);
    }
}
