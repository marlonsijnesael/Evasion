using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public CCTest ccTest;

    public bool firstPerson = true;
    public Transform firstPersonPos, thirdPersonPos;
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;

    private void Start()
    {
        transform.position = thirdPersonPos.position;
        transform.SetParent(thirdPersonPos);
    }
    private void ChangeCam()
    {
        if (!firstPerson)
        {
            transform.position = firstPersonPos.position;
            firstPerson = true;
        }
        else
        {
            transform.position = thirdPersonPos.position;
            firstPerson = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            ChangeCam();
        }
        if (ccTest.isSliding || ccTest.isClimbing || ccTest.isWallrun_Left || ccTest.isWallrun_Right) {
            return;
        }
        else {
            float mouseX = Input.GetAxis("HorizontalLook") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("VerticalLook") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //transform.Rotate(Vector3.left * mouseY);
            playerBody.Rotate(Vector3.up, mouseX);
        }
    }

}
