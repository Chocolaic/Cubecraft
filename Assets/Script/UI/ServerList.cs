using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerList : MonoBehaviour
{
    [SerializeField]
    public GameObject serverbox;
    public GameObject content;
    public GameObject preItem;

    private Servers save;
    public void BtnConfirm_Click()
    {
        string host = GameObject.Find("input_host").GetComponent<InputField>().text;
        InputCompleted(host);
    }
    // Start is called before the first frame update
    void Start()
    {
        save = Manager.LoadData<Servers>("servers");
        foreach(string node in save.List)
        {
            AddServer(node);
        }
        serverbox.SetActive(false);
    }
    public void InputCompleted(string host)
    {
        save.List.Add(host);
        Manager.SaveData("servers", save);
        AddServer(host);
    }
    private void AddServer(string host)
    {
        serverbox.SetActive(false);
        GameObject item = Instantiate(preItem);
        item.transform.SetParent(content.transform);
        RectTransform transform = item.GetComponent<RectTransform>();
        transform.offsetMax = new Vector2(0, 200);
        transform.offsetMin = new Vector2(-1800, 0);
        item.GetComponent<ServerItem>().SetInfo(host);

    }

    public void RemoveServer(GameObject item)
    {
        save.List.Remove(item.GetComponent<ServerItem>().host);
        Destroy(item);
        Manager.SaveData("servers", save);
    }

    public void ToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
