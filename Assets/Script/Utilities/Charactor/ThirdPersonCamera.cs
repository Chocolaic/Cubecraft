using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float zoomSpeed = 1f;
    public float minDistance = 5f;
    public float maxDistance = 10f;
    public float xSpeed = 250.0f;
    public Vector3 pivotOffset = Vector3.zero;
    public float yMinLimit = 30f;
    public float yMaxLimit = 80f;

    Transform maincamera;
    private float targetDistance = 0f;
    private float targetX = 0f;
    private float targetY = 0f;
    private float x = 0.0f;
    private float y = 0.0f;
    private float xVelocity = 1f;
    private float distance;
    private float zoomVelocity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        maincamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        var angles = maincamera.eulerAngles;
        targetY = y = ClampAngle(angles.y, yMinLimit, yMaxLimit);
        targetDistance = distance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Z)) targetDistance -= zoomSpeed;
        else if (Input.GetKey(KeyCode.X)) targetDistance += zoomSpeed;
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        if(Input.GetKey(KeyCode.E))
            targetX += xSpeed * 0.02f;
        else if(Input.GetKey(KeyCode.Q))
            targetX -= xSpeed * 0.02f;

        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
        y = targetY;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.5f);

        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + transform.position + pivotOffset;
        maincamera.rotation = rotation;
        maincamera.position = position;
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
