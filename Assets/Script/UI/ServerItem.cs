using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerItem : MonoBehaviour
{
    [SerializeField]
    public GameObject texthost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetInfo(string host)
    {
        texthost.GetComponent<Text>().text = host;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
