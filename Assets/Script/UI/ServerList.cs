using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerList : MonoBehaviour
{
    [SerializeField]
    public GameObject serverbox;
    public GameObject content;
    public GameObject preItem;

    public void BtnConfirm_Click()
    {
        string host = GameObject.Find("host").GetComponent<Text>().text;
        AddServer(host);
    }
    // Start is called before the first frame update
    void Start()
    {
        serverbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddServer(string host)
    {
        serverbox.SetActive(false);
        Debug.Log(host);
        GameObject item = Instantiate(preItem);
        item.transform.SetParent(content.transform);
        RectTransform transform = item.GetComponent<RectTransform>();
        //Debug.Log(posY);
        transform.offsetMax = new Vector2(0, 200);
        transform.offsetMin = new Vector2(-1800, 0);
        item.GetComponent<ServerItem>().GetInfo(host);
        //transform.anchoredPosition = new Vector2(0, posY);
    }
}
