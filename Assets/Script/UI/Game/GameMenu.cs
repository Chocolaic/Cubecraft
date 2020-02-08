using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToMenu()
    {
        GameObject.Find("Network").GetComponent<NetWorkManage>().Disconnect();
        SceneManager.LoadSceneAsync("Menu");
        System.GC.Collect();
    }
}
