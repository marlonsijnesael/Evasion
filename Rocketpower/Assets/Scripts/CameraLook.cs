using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    //Make sure you have a camera, it will determine the direction the character faces
    Transform cam;

    float speed = 10f;    //How fast the player can move
    float turnSpeed = 100;    //How fast the player can rotate

    Animator animator;//You may not need an animator, but if so declare it here
    public GameObject player;
    void Start()
    {
        cam = Camera.main.transform;
    }

    //No need for update function right now, physics work better in Fixed Update
    void Update()
    {
    }

    public void UpdateRotation()
    {
        //right is shorthand for (1,0,0) or the x value            forward is short for (0,0,1) or the z value 
        Vector3 dir = (cam.right * Input.GetAxis("Horizontal")) + (cam.forward * Input.GetAxis("Vertical"));

        dir.y = 0;//Keeps character upright against slight fluctuations

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //rotate from this /........to this............../.........at this speed 
            player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        }
    }
}


//     public float mouseSensitivity = 100.0f;
//     public float clampAngle = 80.0f;

//     private float rotY = 0.0f; // rotation around the up/y axis
//     private float rotX = 0.0f; // rotation around the right/x axis
//     public GameObject player;
//     private void Start()
//     {
//         Vector3 rot = transform.localRotation.eulerAngles;
//         rotY = rot.y;
//         rotX = rot.x;
//     }

//     private void Update()
//     {
//         float mouseX = Input.GetAxis("HorizontalLook");
//         float mouseY = Input.GetAxis("VerticalLook");

//         rotY += mouseX * mouseSensitivity * Time.deltaTime;
//         rotX += mouseY * mouseSensitivity * Time.deltaTime;

//         rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

//         Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
//         transform.localRotation = localRotation;

//         Vector3 joyStick = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//         float heading = Mathf.Atan2(joyStick.x, joyStick.z);
//         player.transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0.0f);



//     }
// }

// {
//     public StateMachine ccTest;

//     public bool firstPerson = true;
//     public Transform firstPersonPos, thirdPersonPos;
//     public float lookSensitivity = 100f;
//     public Transform playerBody;
//     private float xRotation = 0f;
//     private float yRotation = 0f;
//     private float xRotationV;
//     private float yRotationV;
//     private float LookSmoothDamp = 0.1f;

//     private float currentXRotation;
//     private float currentYRotation;

//     private void Start()
//     {
//         transform.position = thirdPersonPos.position;
//         transform.SetParent(thirdPersonPos);
//     }
//     private void ChangeCam()
//     {
//         if (!firstPerson)
//         {
//             transform.position = firstPersonPos.position;
//             firstPerson = true;
//         }
//         else
//         {
//             transform.position = thirdPersonPos.position;
//             firstPerson = false;
//         }
//     }

//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.LeftShift))
//         {
//             ChangeCam();
//         }
//         if (ccTest.isSliding || ccTest.isClimbing || ccTest.isWallrun_Left || ccTest.isWallrun_Right)
//         {
//             return;
//         }
//         else if (firstPerson == true)
//         {
//             float rStickX = Input.GetAxis("HorizontalLook") * lookSensitivity /** Time.deltaTime*/;
//             float rStickY = 0 * Input.GetAxis("VerticalLook") * lookSensitivity /** Time.deltaTime*/;

//             yRotation += rStickY;
//             if (yRotation > 60.0f)
//             {
//                 yRotation = 60.0f;
//                 rStickY = 0.0f;
//             }
//             if (yRotation < -40.0f)
//             {
//                 yRotation = -40.0f;
//                 rStickY = 0.0f;
//             }

//             transform.Rotate(Vector3.left * rStickY);
//             playerBody.Rotate(Vector3.up, rStickX);
//         }
//         else
//         {
//             float rStickX = Input.GetAxis("HorizontalLook") * lookSensitivity /** Time.deltaTime*/;
//             float rStickY = 0 * Input.GetAxis("VerticalLook") * lookSensitivity /** Time.deltaTime*/;

//             yRotation += rStickY;
//             if (yRotation > 50.0f)
//             {
//                 yRotation = 50.0f;
//                 rStickY = 0.0f;
//             }
//             if (yRotation < -30.0f)
//             {
//                 yRotation = -30.0f;
//                 rStickY = 0.0f;
//             }

//             //transform.Rotate(Vector3.left * rStickY);
//             playerBody.Rotate(Vector3.up, rStickX);
//         }
//     }

// }
