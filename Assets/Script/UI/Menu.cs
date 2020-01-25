using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    public GameObject maincamera, camFocus;

    public Vector3 pivotOffset = Vector3.zero; // offset from target's pivot
    public Transform target; // like a selected object (used with checking if objects between cam and target)

    public float distance = 10.0f; // distance from target (used with zoom)

    public float xSpeed = 50.0f;
    public float ySpeed = 120.0f;

    public bool allowYTilt = true;
    public float yMinLimit = 30f;
    public float yMaxLimit = 80f;

    private float x = 0.0f;
    private float y = 10.0f;

    private float targetX = 0f;
    private float targetDistance = 10f;
    private float xVelocity = 1f;
    private float zoomVelocity = 1f;
    public void BtnOffline_Click()
    {
        Debug.Log("OfflineMode");
        SceneManager.LoadSceneAsync("MapInstance");
    }
    public void BtnOnline_Click()
    {
        Debug.Log("OnlineMode");
        SceneManager.LoadSceneAsync("ServerList");
    }

    public void BtnExit_Click()
    {
        Application.Quit();
    }

    void Start()
    {
        var angles = transform.eulerAngles;
        targetX = x = angles.x;
        targetDistance = distance;
    }

    void LateUpdate()
    {
        if (camFocus.transform)
        {
            targetX += xSpeed * 0.02f;
            x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.5f);

            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + camFocus.transform.position + pivotOffset;
            maincamera.transform.rotation = rotation;
            maincamera.transform.position = position;

        }
    }
}
