using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Determines the limitations of vertical camera movement
    private const float Y_ANGLE_MIN = 0f;
    private const float Y_ANGLE_MAX = 40.0f;
    public Transform character; //What the camera is looking at..the main character
    public Transform lookAt;
    public float distanceZ = -5.0f; // Distance to stay from character, Make sure it is negative
    public float distanceY = 0f;
    private float currentX = 0.0f; // Holds value of X mouse movement
    private float currentY = 0.0f; // Holds value of Y mouse movement
    public float sensitivity;
    public VirtualController virtualController;
    private Vector3 velocity = Vector3.zero;
    public bool locked;
    public float smoothTime;
    private Camera cam;

    private Vector3 offsetPosition;
    private Space offsetPositionSpace = Space.Self;
    public bool smoothLerp = false;
    private void Awake()
    {
        // virtualController = character.GetComponent<VirtualController>();
        cam = GetComponent<Camera>();
    }


    private void FixedUpdate()
    {
        if (smoothLerp)
            SmoothFollow();
        else
            LerpFollow();
    }


    public void SmoothFollow()
    {
        Debug.DrawRay(character.transform.position + Vector3.up, character.forward, Color.red);

        Vector3 targetPosition = character.TransformPoint(new Vector3(0, distanceY, distanceZ));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.LookAt(character.position);//Points camera at character   

        float hLook = virtualController.HorizontalLook;
        float vLook = virtualController.VerticalLook;
        if (Mathf.Abs(hLook) > 0.05f || Mathf.Abs(vLook) > 0.05f)
        {
            currentX += sensitivity * hLook;
            currentY += sensitivity * vLook;
        }
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

    }

    public void LerpFollow()
    {
        offsetPosition = new Vector3(0, distanceY, distanceZ);
        if (character == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            //transform.position = target.TransformPoint(offsetPosition);
            transform.position = Vector3.Lerp(transform.position, character.TransformPoint(offsetPosition), smoothTime);
        }
        else
        {
            transform.position = character.position + offsetPosition;
        }

        // compute rotation
        transform.LookAt(character);
    }

}
//old but keep!:
// transform.position = Vector3.SmoothDamp(transform.position, lookAt.position + Quaternion.Euler(currentY, currentX, 0) * new Vector3(0, distanceY, distanceZ), ref velocity, 1);