using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField]
    public GameObject textObj, loadObj, btnExitObj;
    public Sprite[] loadingFrames;
    private float interval;
    int pload;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        interval += Time.deltaTime;
        if(loadObj.active && interval > 0.3)
        {
            interval = 0;
            loadObj.GetComponent<Image>().sprite = loadingFrames[pload];
            pload = (pload + 1) % loadingFrames.Length;
        }
    }
    public void ShowText(string text, bool exitbtn)
    {
        btnExitObj.SetActive(exitbtn);
        loadObj.SetActive(!exitbtn);
        textObj.GetComponent<Text>().text = text;
    }
}
