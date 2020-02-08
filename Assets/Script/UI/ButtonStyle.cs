using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonStyle : MonoBehaviour
{
    [SerializeField]
    public Sprite normalImg, activeImg;
    public GameObject textObj;
    public string normalFront, activeFront;
    Image btnImg;
    public void MouseEnter()
    {
        if (activeImg != null)
            btnImg.sprite = activeImg;
        textObj.GetComponent<Text>().text = activeFront;
    }
    public void MouseLeave()
    {
        if (normalImg != null)
            btnImg.sprite = normalImg;
        textObj.GetComponent<Text>().text = normalFront;
    }
    void Start()
    {
        btnImg = GetComponent<Image>();
    }
}
