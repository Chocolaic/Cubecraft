using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonInteraction : MonoBehaviour
{
    public float walkSpeed = 10;
    public float jumpSpeed = 5;
    internal IPlayerInteraction interact;

    public float mousespeed = 5f;
    public float maxmouseY = 45f;
    public float minmouseY = -45f;
    public Transform cameraObj;

    Camera fpscamera;
    float RotationX, RotationY;
    bool lockMouse = true;
    Vector3 sight = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
    CharacterController controller;
    Vector3 direction = Vector3.zero;
    Vector3 recordPosition;

    LayerMask chunklayer;
    // Start is called before the first frame update
    void Start()
    {
        this.chunklayer = 1 << LayerMask.NameToLayer("Chunk");
        this.fpscamera = cameraObj.GetComponent<Camera>();
        this.controller = GetComponent<CharacterController>();
        this.sight = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
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
            RotationX += cameraObj.localEulerAngles.y + Input.GetAxis("Mouse X") * mousespeed;
            RotationY -= Input.GetAxis("Mouse Y") * mousespeed;
            RotationY = Mathf.Clamp(RotationY, minmouseY, maxmouseY);

            this.transform.eulerAngles = new Vector3(0, RotationX, 0);
            cameraObj.eulerAngles = new Vector3(RotationY, RotationX, 0);

            if (Input.GetMouseButtonDown(0))
            {
                SelectBlock();
            }
        }
    }
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
            interact.UpdatePosition(onGround, transform.position.x, onGround ? (int)transform.position.y : transform.position.y, transform.position.z);
    }
    void SelectBlock()
    {
        RaycastHit hit;
        Ray ray = fpscamera.ScreenPointToRay(sight);
        if (Physics.Raycast(ray, out hit, 5, chunklayer))
        {
            Vector3 pos = hit.point + (ray.direction.normalized * 0.5f);
            Chunk chunk = hit.transform.GetComponent<Chunk>();
            interact.BreakSelectBlock(chunk, pos);
            Debug.Log("Selected X:" + pos.x + " Y:" + pos.y + " Z:" + pos.z);
        }
    }
    void ReplaceAndUseBlock()
    {
        RaycastHit hit;
        Ray ray = fpscamera.ScreenPointToRay(sight);
        if (Physics.Raycast(ray, out hit, 5, chunklayer))
        {
            Vector3 pos = hit.point + (hit.normal.normalized * 0.5f);
            Chunk chunk = hit.transform.GetComponent<Chunk>();
        }
    }
    public void Move(Vector3 position)
    {
        transform.position = position;
        recordPosition = position;
    }
}
