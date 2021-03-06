﻿using UnityEngine;

public class SwitchCameraController : MonoBehaviour
{
    public bool firstPerson = true;
    public Transform firstPersonPos, thirdPersonPos;

    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    private void Awake()
    {
        // LockCursor();
        xAxisClamp = 0.0f;
    }

    private void Start()
    {
        transform.position = firstPersonPos.position;
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

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeCam();
        }
    }

    private void CameraRotation()
    {
        Debug.Log("Horizontal");
        float mouseX = Input.GetAxis("Horizontal") * mouseSensitivity * Time.deltaTime;
        float mouseY = 0;//Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}