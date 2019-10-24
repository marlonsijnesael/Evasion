
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

    public Vector3 wallrunDir = Vector3.zero;
    private Vector3 vel = Vector3.zero;
    private bool wallrun;
    private bool grounded;
    private RaycastHit verticalTest;
    public AnimContrller animationController;
    private Vector3 verticalVel, horizontalVel;
    public float gravity = 14.0f;
    public float jumpVel = 14f;
    private bool slide;
    public float slideForce = 10f;

    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    public void FixedUpdate()
    {
        vel = initVelocity;

        CheckGrounded();
        CheckInput();
        GroundTest();
        SlideDown();
        Jump();
        LeftRightCollisions();
        if (!wallrun && !slide)
            Run();
        Reset();
    }

    private void Accelerate(float rate)
    {
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    private void GroundTest()
    {
        Physics.Raycast(transform.position, -transform.up, out verticalTest);
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

        if (Input.GetKey(KeyCode.X) && grounded)
        {
            slide = true;
        }
        else
        {
            slide = false;
        }
    }

    private void LeftRightCollisions()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, 1 + controller.skinWidth))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) == 0)
            {
                wallrunDir = Vector3.Cross(hit.normal, Vector3.up);
                Debug.DrawRay(hit.point, wallrunDir, Color.red, 10f);
                wallrun = true;
                WallRun();
            }
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, 1 + controller.skinWidth))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) == 0)
            {
                Debug.Log("left wall" + hit.transform.CompareTag("wall"));

                wallrunDir = Vector3.Cross(Vector3.up, hit.normal);
                Debug.DrawRay(hit.point, wallrunDir, Color.red, 10f);
                wallrun = true;
                WallRun();
            }
        }
        else
        {
            wallrunDir = Vector3.zero;
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
            vel += force + horizontalVel + (transform.rotation * Vector3.forward - wallrunDir.normalized) * forwardVelocity;
            controller.Move((vel) * Time.deltaTime);
        }
        animationController.SetBool(anim, AnimContrller.animations.wallrun_right.ToString(), wallrun);
        animationController.SetBool(anim, AnimContrller.animations.running.ToString(), wallrun);
    }

    private void SlideDown()
    {
        vel = Vector3.zero;
        Vector3 force = Vector3.zero;
        if (slide)
        {
            vel += (transform.rotation * Vector3.forward) * slideForce;
            controller.Move((vel + verticalVel) * Time.deltaTime);
        }
        animationController.SetBool(anim, AnimContrller.animations.running.ToString(), slide);
        animationController.SetBool(anim, AnimContrller.animations.sliding.ToString(), slide);
    }

    private Vector3 CalcForward
    {
        get
        {
            if (grounded)
            {

                Vector3 cross = Vector3.Cross(verticalTest.normal, -transform.right);
                Debug.DrawRay(verticalTest.point, cross, Color.red, 10f);
                return cross;
            }
            return transform.forward;
        }
    }

    private void Run()
    {
        if (!grounded && !wallrun)
        {
            verticalVel.y -= gravity * Time.deltaTime;
        }
        vel += (transform.rotation * Vector3.forward) * forwardVelocity;
        animationController.SetBool(anim, AnimContrller.animations.running.ToString(), vel.z != 0);
        animationController.SetBool(anim, AnimContrller.animations.wallrun_right.ToString(), false);
        animationController.SetBool(anim, AnimContrller.animations.wallrun_left.ToString(), false);
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
