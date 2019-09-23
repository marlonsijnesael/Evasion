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


    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 newRotation = new Vector3();
        rightLeftRotation += InputCatcher.GetAxis("rightHorizontal", 1) * sensitivity * Time.deltaTime;

        upDownRotation += InputCatcher.GetAxis("rightVertical", 1) * sensitivity * Time.deltaTime;
        upDownRotation = Mathf.Clamp(upDownRotation, -maxLookUpDownAngle, maxLookUpDownAngle);
        // getting the newRotation vector out of our left/right and up/down mouse values
        // remember we rotate up/down around the x-axis and left/right around the y-axis
        newRotation = new Vector3(upDownRotation, rightLeftRotation, 0);
        transform.localRotation = Quaternion.Euler(newRotation);
    }
}

