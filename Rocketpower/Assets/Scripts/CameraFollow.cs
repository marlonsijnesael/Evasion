using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Determines the limitations of vertical camera movement
    private const float Y_ANGLE_MIN = 15.0f;
    private const float Y_ANGLE_MAX = 25.0f;

    public Transform character; //What the camera is looking at..the main character

    private float distance = -5.0f; // Distance to stay from character, Make sure it is negative
    private float currentX = 0.0f; // Holds value of X mouse movement
    private float currentY = 0.0f; // Holds value of Y mouse movement
    public float sensitivity;
    void start() { }

    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("HorizontalLook")) > 0.1f || Mathf.Abs(Input.GetAxis("VerticalLook")) > 0.1f)
        {
            currentX += sensitivity * Input.GetAxis("HorizontalLook");
            currentY += sensitivity * Input.GetAxis("VerticalLook");
        }

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    void LateUpdate()
    {                                                        //Rotation around character............/...Keeps distance from character          
        gameObject.transform.position = character.position + Quaternion.Euler(currentY, currentX, 0) * new Vector3(0, 0, distance);
        gameObject.transform.LookAt(character.position);//Points camera at character
    }
}
