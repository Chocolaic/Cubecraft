using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    private float interval;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        interval += Time.deltaTime;
        if(interval > 3)
        {
            interval = 0;
            GetComponent<Text>().text = "";
            gameObject.SetActive(false);
        }
    }
    public void Show(string text)
    {
        GetComponent<Text>().text = text;
    }
}
