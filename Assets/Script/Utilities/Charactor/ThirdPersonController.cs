using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public float JumpSpeed = 5.0f;
    Transform m_camera;
    CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {  
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W))
                    controller.transform.eulerAngles = new Vector3(0, m_camera.transform.eulerAngles.y, 0);
                if (Input.GetKey(KeyCode.S))
                    controller.transform.eulerAngles = new Vector3(0, m_camera.transform.eulerAngles.y + 180f, 0);
                if (Input.GetKey(KeyCode.A))
                    controller.transform.eulerAngles = new Vector3(0, m_camera.transform.eulerAngles.y + 270f, 0);
                if (Input.GetKey(KeyCode.D))
                    controller.transform.eulerAngles = new Vector3(0, m_camera.transform.eulerAngles.y + 90f, 0);
                moveDirection = transform.forward;
                //moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= MoveSpeed;
            }
            else
                moveDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.Space))
                moveDirection.y = JumpSpeed;
        }
        else
            moveDirection.y -= 10 * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
