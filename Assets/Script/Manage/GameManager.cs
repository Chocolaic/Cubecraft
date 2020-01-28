using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject msgBox;
    public GameObject chatBoxContent;
    public ScrollRect chatBox;
    public GameObject chatBoxText;
    public GameObject chatInput;

    List<GameObject> chatList = new List<GameObject>();

    bool _inputEnable;
    bool InputEnable { get
        {
            _inputEnable = !_inputEnable; return _inputEnable;
        } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            chatInput.SetActive(InputEnable);
        }
    }
    public void AddChatText(string text)
    {
        GameObject obj = Instantiate(chatBoxText);
        obj.GetComponent<Text>().text = text;
        obj.transform.SetParent(chatBoxContent.transform);
        chatList.Add(obj);
        while (chatList.Count > 64)
        {
            GameObject tmp = chatList[0];
            chatList.RemoveAt(0);
            Destroy(tmp);
        }
        Canvas.ForceUpdateCanvases();
        chatBox.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }
    public void ChatInputCompleted(string text)
    {
        AddChatText(text);
        chatInput.GetComponent<InputField>().text = "";
        chatInput.SetActive(InputEnable);
    }
    public void ShowMsg(string text)
    {
        msgBox.SetActive(true);
        msgBox.GetComponent<MessageBox>().Show(text);
    }
}
