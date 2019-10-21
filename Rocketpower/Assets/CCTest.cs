using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTest : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 initVelocity = new Vector3(0, -1, 0);
    public Transform startPoint;

    [Header("Acceleration Settings: ")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;

    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0;

    [Header("Scene objects: ")]
    [SerializeField] private Animator anim;

    public Vector3 orthogonal = Vector3.zero;
    private Vector3 vel = Vector3.zero;
    private bool wallrun;
    private bool grounded;
    private RaycastHit verticalTest;
    public AnimContrller animationController;

    public bool useGravityWhileRunning = false;

    private Vector3 verticalVel, horizontalVel;
    public float gravity = 14.0f;
    public float jumpVel = 14f;

    [SerializeField] private float sensitivity = 5;

    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    private void Update()
    {
        RotateBody();
    }

    public void FixedUpdate()
    {
        vel = initVelocity;

        CheckGrounded();
        CheckInput();
        GroundTest();

        Jump();
        LeftRightCollisions();
        if (!wallrun)
            Run();
        Reset();
        Debug.DrawRay(transform.position, transform.rotation * Vector3.forward, Color.red);

    }

    private void Accelerate(float rate)
    {
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
        // vel.z += forwardVelocity;
    }

    private void GroundTest()
    {
        if (Physics.Raycast(transform.position, transform.up, out verticalTest))
        {
            Debug.DrawRay(transform.position, transform.up, Color.red);
        }
    }

    private void CheckGrounded()
    {
        grounded = controller.isGrounded;
        animationController.SetBool(anim, AnimContrller.animations.airtime.ToString(), grounded);
    }

    private void CheckInput()
    {
        if (!Input.GetKey(KeyCode.UpArrow))
        {
            Accelerate(decelRatePerSec);
        }

        else
        {
            Accelerate(accelRatePerSec);
        }
    }

    private void LeftRightCollisions()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, 1 + controller.skinWidth))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) == 0)
            {
                orthogonal = Vector3.Cross(hit.normal, Vector3.up);
                Debug.DrawRay(hit.point, orthogonal, Color.red, 10f);
                wallrun = true;
                WallRun();
            }
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, 1 + controller.skinWidth))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) == 0)
            {
                Debug.Log("left wall" + hit.transform.CompareTag("wall"));

                orthogonal = Vector3.Cross(Vector3.up, hit.normal);
                Debug.DrawRay(hit.point, orthogonal, Color.red, 10f);
                wallrun = true;
                WallRun();
            }
        }
        else
        {
            orthogonal = Vector3.zero;
            wallrun = false;
        }
    }


    private void Jump()
    {
        if (wallrun && Input.GetKeyDown(KeyCode.Space))
        {
            horizontalVel.x = -5;
        }


        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVel.y = jumpVel;
        }
    }

    private void WallRun()
    {
        vel = Vector3.zero;
        Vector3 force = Vector3.zero;
        if (wallrun)
        {
            if (!grounded && !Input.GetKey(KeyCode.X) && useGravityWhileRunning)
            {
                force.y = -5;

            }
            else if (!grounded && Input.GetKey(KeyCode.X))
            {
                force.y = 5;
            }

            vel += force + horizontalVel + (transform.rotation * Vector3.forward - orthogonal.normalized) * forwardVelocity;

            animationController.SetBool(anim, AnimContrller.animations.running.ToString(), wallrun);
            animationController.SetBool(anim, AnimContrller.animations.wallrun_right.ToString(), wallrun);

            controller.Move((vel) * Time.deltaTime);
        }
    }
    float rightLeftRotation = 0f;
    private void RotateBody()
    {
        // // Quaternion newRot = new Quaternion();
        // rightLeftRotation = Input.GetAxisRaw("Horizontal") * sensitivity * Time.deltaTime;
        // Vector3 rotation = new Vector3(0, rightLeftRotation, 0);
        // // if (grounded)
        // // {
        // //     newRot = Quaternion.FromToRotation(transform.up, verticalTest.normal) * transform.rotation;
        // // }

        // // else
        // // {
        // //     newRot = Quaternion.FromToRotation(Vector3.zero, verticalTest.normal) * transform.rotation;

        // // }

        // // // newRot = Quaternion.Euler(rotation) * newRot;
        // // transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime);
        // Quaternion newRot = Quaternion.FromToRotation(transform.up, -verticalTest.normal) * transform.rotation * Quaternion.Euler(rotation);
        // transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * sensitivity);


        float rotationSpeed = 25.0f;
        if (Input.GetKey("escape"))
            Application.Quit();

        float rotation = Input.GetAxisRaw("Horizontal") * rotationSpeed;

        rotation *= Time.deltaTime;

        transform.Rotate(0, rotation, 0);
    }




    private Vector3 CalcForward
    {
        get
        {
            if (grounded)
            {
                return Vector3.Cross(verticalTest.normal, -transform.right);
            }
            return transform.forward;
        }
    }

    private void CalcMoveDir()
    {

    }

    private void Run()
    {

        if (!grounded && !wallrun)
        {
            verticalVel.y -= gravity * Time.deltaTime;
        }
        vel += (transform.rotation * Vector3.forward) * forwardVelocity;
        // vel = transform.TransformDirection(vel);
        animationController.SetBool(anim, AnimContrller.animations.running.ToString(), vel.z != 0);
        animationController.SetBool(anim, AnimContrller.animations.wallrun_right.ToString(), false);
        animationController.SetBool(anim, AnimContrller.animations.wallrun_left.ToString(), false);
        // Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")); //+ transform.forward;
        controller.Move((vel + verticalVel) * Time.deltaTime);
    }


    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            vel = Vector3.zero;
            verticalVel = Vector3.zero;
            horizontalVel = Vector3.zero;
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
        }
    }


}
