using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform cam;
    [SerializeField] private float sensitivity = 5;
    [SerializeField] private float maxLookUpDownAngle = 60; // can't look up/down more than 60 dgs

    private Vector3 pos;
    private float upDownRotation = 0.0f;
    private float rightLeftRotation = 0.0f;

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 newRotation = new Vector3();
        //rightLeftRotation += InputCatcher.GetAxis("rightHorizontal", 1) * sensitivity * Time.deltaTime;
        rightLeftRotation += Input.GetAxisRaw("Horizontal") * sensitivity * Time.deltaTime;

        //upDownRotation += InputCatcher.GetAxis("rightVertical", 1) * sensitivity * Time.deltaTime;
      //  upDownRotation += Input.GetAxisRaw("Vertical") * sensitivity * Time.deltaTime;

        upDownRotation = Mathf.Clamp(upDownRotation, -maxLookUpDownAngle, maxLookUpDownAngle);

        newRotation = new Vector3(upDownRotation, rightLeftRotation, 0);
        cam.transform.localRotation = Quaternion.Euler(newRotation);
    }

    


}

