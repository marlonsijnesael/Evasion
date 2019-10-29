using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public bool firstPerson = true;
    public Transform firstPersonPos, thirdPersonPos;
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;

    public bool lockCam = false;

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
        if (lockCam)
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeCam();
        }
        float mouseX = Input.GetAxis("Horizontal") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Vertical") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up, mouseX);
    }

}
