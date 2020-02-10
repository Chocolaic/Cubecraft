using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 10;
    public float jumpSpeed = 5;
    internal IPlayerAction action;
    CharacterController controller;
    Vector3 direction = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        bool onGround = controller.isGrounded;
        if (onGround)
        {
            direction = new Vector3(_horizontal, 0, _vertical);
            direction *= walkSpeed;
            if (Input.GetKey(KeyCode.Space))
            {
                direction.y = jumpSpeed;
            }
        }
        else
            direction.y -= 10f * Time.deltaTime;

        controller.Move(controller.transform.TransformDirection(direction * Time.deltaTime));
        if (direction != Vector3.zero)
            action.UpdatePosition(onGround, transform.position.x, onGround ? (int)transform.position.y : transform.position.y, transform.position.z);
    }
}
