using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public CCTest ccTest;

    public bool firstPerson = true;
    public Transform firstPersonPos, thirdPersonPos;
    public float lookSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float xRotationV;
    private float yRotationV;
    private float LookSmoothDamp = 0.1f;

    private float currentXRotation;
    private float currentYRotation;

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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeCam();
        }
        if (ccTest.isSliding || ccTest.isClimbing || ccTest.isWallrun_Left || ccTest.isWallrun_Right) {
            return;
        }
        else if(firstPerson == true)
        {
            float rStickX = Input.GetAxis("HorizontalLook") * lookSensitivity /** Time.deltaTime*/;
            float rStickY = Input.GetAxis("VerticalLook") * lookSensitivity /** Time.deltaTime*/;

            yRotation += rStickY;
            if (yRotation > 60.0f) {
                yRotation = 60.0f;
                rStickY = 0.0f;
            }
            if (yRotation < -40.0f) {
                yRotation = -40.0f;
                rStickY = 0.0f;
            }

            transform.Rotate(Vector3.left * rStickY);
            playerBody.Rotate(Vector3.up, rStickX);
        }
        else {
            float rStickX = Input.GetAxis("HorizontalLook") * lookSensitivity /** Time.deltaTime*/;
            float rStickY = Input.GetAxis("VerticalLook") * lookSensitivity /** Time.deltaTime*/;

            yRotation += rStickY;
            if (yRotation > 50.0f) {
                yRotation = 50.0f;
                rStickY = 0.0f;
            }
            if (yRotation < -30.0f) {
                yRotation = -30.0f;
                rStickY = 0.0f;
            }

            transform.Rotate(Vector3.left * rStickY);
            playerBody.Rotate(Vector3.up, rStickX);
        }
    }

}
