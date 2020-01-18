
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldController : MonoBehaviour
{
    public GroundChecker groundChecker;

    public float climbSpeed;
    public GameObject head;
    public Vector3 headRot = new Vector3(0, 0, 45);

    [Header("Character properties")]
    public CharacterController controller;
    public Animator anim;
    [SerializeField] private AnimationManager animationController;
    [Header("Acceleration Settings: ")]
    public float maxSpeed = 5f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0;

    [Header("gravity Settings: ")]
    [SerializeField] private float gravity = 14.0f;
    [SerializeField] private float jumpVel = 14f;
    [SerializeField] private float slideForce = 10f;

    #region private variables
    private Vector3 wallrunDir = Vector3.zero;
    private Vector3 vel = Vector3.zero;
    private bool grounded;
    private RaycastHit verticalTest;
    private Vector3 verticalVel, horizontalVel;
    private Vector3 initVelocity = new Vector3(0, -1, 0);
    private Vector3 tmpVel = Vector3.zero;
    public GameObject prefab;
    public CharacterController cc;
    public RotatePlayer cameraLook;
    #endregion

    #region animator booleans
    [HideInInspector] public bool isSliding;
    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool isWallrun_Right;
    [HideInInspector] public bool isWallrun_Left;
    #endregion


    #region  monobehaviour functions
    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    public void FixedUpdate()
    {
        vel = initVelocity;

        GroundTest();
        if (!grounded && !isWallrun_Right && !isWallrun_Left)
        {
            verticalVel.y -= gravity * Time.deltaTime;
        }
        CheckInput();
        FrontCollisionTest();
        SlideDown();

        Jump();
        LeftRightCollisionsTest();
        if (!isWallrun_Right && !isWallrun_Left && !isSliding && !isClimbing)
            Run();
    }
    #endregion

    #region  Input functions
    private void CheckInput()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            Accelerate(accelRatePerSec);
        }

        else
        {
            Accelerate(decelRatePerSec);
        }

        if (Input.GetButton("Slide"))
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
    }
    private void Accelerate(float rate)
    {
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    private void GroundTest()
    {
        grounded = controller.isGrounded;
        animationController.SetBool(anim, AnimationManager.AnimationStates.airtime.ToString(), grounded);
        Physics.Raycast(transform.position, -transform.up, out verticalTest);
    }

    private void FrontCollisionTest()
    {
        if (isWallrun_Right || isWallrun_Left)
            return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1 + controller.skinWidth))
        {
            var maxBounds = GetBounds.GetMaxBounds(hit.transform.gameObject);
            //Debug.Log(maxBounds.max.y - transform.position.y);
            if (maxBounds.max.y - transform.position.y > 1)
            {
                if (Vector3.Dot(hit.normal, Vector3.up) == 0)
                {

                    isClimbing = true;
                    WallClimb();

                    //                    Debug.DrawRay(hit.point, Vector3.up, Color.blue, 10f);

                }
                else
                {
                    isClimbing = false;
                }
            }
            else
            {
                isClimbing = false;
            }
        }
    }

    private void LeftRightCollisionsTest()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, 1 + controller.skinWidth))
        {
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            //Debug.Log(dot + ": dot");
            if (dot == 0)
            {
                wallrunDir = Vector3.Cross(hit.normal, Vector3.up);
                //Debug.DrawRay(hit.point, wallrunDir, Color.red, 10f);
                isWallrun_Right = true;


                WallRun();
            }
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, 1 + controller.skinWidth))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) == 0)
            {
                Debug.Log("left wall" + hit.transform.CompareTag("wall"));

                wallrunDir = Vector3.Cross(hit.transform.up, hit.normal);
                Debug.DrawRay(hit.point, wallrunDir, Color.red, 10f);
                isWallrun_Left = true;
                WallRun();
            }
        }
        else
        {
            wallrunDir = Vector3.zero;
            isWallrun_Right = false;
            isWallrun_Left = false;
        }
    }
    #endregion

    #region movement functions
    private IEnumerator Climb(Vector3 wallUp)
    {
        vel = Vector3.zero;
        Vector3 force = Vector3.up;
        force.y += climbSpeed;
        animationController.SetBool(anim, AnimationManager.AnimationStates.climbing.ToString(), isClimbing);
        while (isClimbing)
        {
            {
                vel += force;
                controller.Move((vel) * Time.deltaTime);
                yield return null;
            }
        }
        animationController.SetBool(anim, AnimationManager.AnimationStates.climbing.ToString(), isClimbing);
        yield return new WaitForSeconds(1);
        vel = Vector3.zero;

    }


    private void WallClimb()
    {
        if (isClimbing)
        {
            vel = tmpVel + (transform.rotation * Vector3.forward - Vector3.up);
            controller.Move((vel) * Time.deltaTime);
        }

        animationController.SetBool(anim, AnimationManager.AnimationStates.climbing.ToString(), isClimbing);
        animationController.SetBool(anim, AnimationManager.AnimationStates.wallrun_left.ToString(), isWallrun_Left);
        controller.Move((vel) * Time.deltaTime);

    }

    private void Jump()
    {
        if (isWallrun_Right && Input.GetButtonDown("Jump"))
        {
            horizontalVel.x = jumpVel;
        }

        if (isWallrun_Left && Input.GetButtonDown("Jump"))
        {
            horizontalVel.x = -jumpVel;
        }


        if (grounded && Input.GetButtonDown("Jump"))
        {
            verticalVel.y = jumpVel;
        }
    }

    private void WallRun()
    {
        if (isWallrun_Right || isWallrun_Left)
        {
            vel = tmpVel + horizontalVel + (transform.rotation * Vector3.forward - wallrunDir.normalized);
            controller.Move((vel) * Time.deltaTime);
        }

        animationController.SetBool(anim, AnimationManager.AnimationStates.wallrun_right.ToString(), isWallrun_Right);
        animationController.SetBool(anim, AnimationManager.AnimationStates.wallrun_left.ToString(), isWallrun_Left);
        controller.Move((vel) * Time.deltaTime);

    }

    private void SlideDown()
    {
        vel = Vector3.zero;
        Vector3 force = Vector3.zero;
        if (isSliding)
        {
            vel += (transform.rotation * Vector3.forward) * slideForce + groundChecker.groundSlopeDir.normalized;
            controller.Move((vel + verticalVel) * Time.deltaTime);
        }
        animationController.SetBool(anim, AnimationManager.AnimationStates.sliding.ToString(), isSliding);
    }

    private void Run()
    {
        vel += (transform.rotation * Vector3.forward) * forwardVelocity;
        animationController.SetBool(anim, AnimationManager.AnimationStates.running.ToString(), vel.z != 0);
        animationController.SetBool(anim, AnimationManager.AnimationStates.wallrun_right.ToString(), isWallrun_Right);
        animationController.SetBool(anim, AnimationManager.AnimationStates.wallrun_left.ToString(), isWallrun_Left);
        tmpVel = vel;
        controller.Move((vel + verticalVel) * Time.deltaTime);
    }
    #endregion
}
