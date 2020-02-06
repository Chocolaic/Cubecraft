using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonStyle : MonoBehaviour
{
    Image btnImg;
    public void MouseEnter()
    {
        btnImg.sprite = Resources.Load<Sprite>("Textures/UI/actbtn");
    }
    public void MouseLeave()
    {
        btnImg.sprite = Resources.Load<Sprite>("Textures/UI/norbtn");
    }
    void Start()
    {
        btnImg = GetComponent<Image>();
    }
}
