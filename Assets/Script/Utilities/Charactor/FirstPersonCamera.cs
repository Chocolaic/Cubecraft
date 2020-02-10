using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mousespeed = 5f;
    public float maxmouseY = 45f;
    public float minmouseY = -45f;
    public Transform agretctCamera;


    float RotationX, RotationY;
    bool lockMouse = true;

    // Start is called before the first frame update
    void Start()
    {
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockMouse = !lockMouse;
            Screen.lockCursor = lockMouse;
        }
        if (lockMouse)
        {
            RotationX += agretctCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mousespeed;
            RotationY -= Input.GetAxis("Mouse Y") * mousespeed;
            RotationY = Mathf.Clamp(RotationY, minmouseY, maxmouseY);

            this.transform.eulerAngles = new Vector3(0, RotationX, 0);
            agretctCamera.transform.eulerAngles = new Vector3(RotationY, RotationX, 0);
        }
    }
}
