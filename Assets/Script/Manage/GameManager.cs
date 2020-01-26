using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject msgBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowMsg(string text)
    {
        msgBox.SetActive(true);
        msgBox.GetComponent<MessageBox>().Show(text);
    }
}
