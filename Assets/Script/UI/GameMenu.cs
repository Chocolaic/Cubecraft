using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private bool isactive = false;
    bool Active { get
        {
            isactive = !isactive; return isactive;
        } }
    [SerializeField]
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(Active);
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
