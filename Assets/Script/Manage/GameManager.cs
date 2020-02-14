using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject msgBox;
    public GameObject chatBoxContent;
    public ScrollRect chatBox;
    public GameObject chatBoxText;
    public InputField chatInput;
    public GameObject menu;
    public GameObject elements;

    public Image circleHealth;
    public Image circleFood;
    public Text foodValue;
    public Text healthValue;
    List<GameObject> chatList = new List<GameObject>();

    bool _inputEnable;
    bool lockOperation = true;
    private bool menuactive = false;
    bool MenuActive
    {
        get
        {
            menuactive = !menuactive; return menuactive;
        }
    }
    bool InputEnable { get
        {
            _inputEnable = !_inputEnable; return _inputEnable;
        } }

    // Update is called once per frame
    void Update()
    {
        if (!lockOperation)
        {
            if (Input.GetKeyDown(KeyCode.T) && !_inputEnable)
            {
                chatInput.gameObject.SetActive(InputEnable);
                chatInput.ActivateInputField();
            }
            else if (_inputEnable && !chatInput.isFocused)
                chatInput.gameObject.SetActive(InputEnable);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menu.SetActive(MenuActive);
            }
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
        chatInput.text = "";
    }
    public void InterruptGame(string text)
    {
        HideAllElements();
        lockOperation = true;
        msgBox.SetActive(true);
        msgBox.GetComponent<MessageBox>().ShowText(text, true);
    }
    public void EndLoading()
    {
        lockOperation = false;
        msgBox.SetActive(false);
        ShowAllElements();
    }
    public void HideAllElements()
    {
        elements.SetActive(false);
        Screen.lockCursor = false;
    }
    public void ShowAllElements()
    {
        elements.SetActive(true);
    }
    public void SetHealthValue(float point)
    {
        if(point > 60)
            healthValue.text = ColorUtility.Set(ColorUtility.Green, "HP" + point);
        else if(point > 30)
            healthValue.text = ColorUtility.Set(ColorUtility.Yellow, "HP" + point);
        else
            healthValue.text = ColorUtility.Set(ColorUtility.Red, "HP" + point);
        circleHealth.fillAmount = point / 100;
    }
    public void SetFoodValue(float point)
    {
        foodValue.text = ColorUtility.Set(ColorUtility.Aqua, "FOOD" + point);
        circleFood.fillAmount = point / 100;
    }
}
