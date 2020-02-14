using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonStyle : MonoBehaviour
{
    [SerializeField]
    public Sprite normalImg, activeImg;
    public Text textCom;
    public int normalTextSize, activeTextSize;
    public string normalFront, activeFront;
    Image btnImg;
    public void MouseEnter()
    {
        if (activeImg != null)
            btnImg.sprite = activeImg;
        textCom.text = activeFront;
        textCom.fontSize = activeTextSize;
    }
    public void MouseLeave()
    {
        if (normalImg != null)
            btnImg.sprite = normalImg;
        textCom.text = normalFront;
        textCom.fontSize = normalTextSize;
    }
    void Start()
    {
        btnImg = GetComponent<Image>();
    }
}
