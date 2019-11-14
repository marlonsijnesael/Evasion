using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public int playerID;
    public LedgeDetection ledgeDetector;
    public CameraLook playerRotator;
    public AnimContrller animationController;
    public Animator animator;
    public VirtualController virtualController;
    public Move idleMove, runMove, wallrunMoveRight, wallRunMoveLeft, ClimbMove, SlideMove, airborneMove;
    public enum State { IDLE, RUN, WALLRUN_RIGHT, WALLRUN_LEFT, CLIMB, SLIDE, AIRBORNE }
    public State playerState = new State();
    public CharacterController cc;
    [HideInInspector] public Vector3 verticalDir = Vector3.zero;
    [HideInInspector] public Vector3 wallrunDir;
    public Vector3 moveDir = Vector3.zero;

    #region animator booleans
    [HideInInspector] public bool isSliding;
    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool isWallrun_Right;
    [HideInInspector] public bool isWallrun_Left;
    public float maxSpeed = 5f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    public float forwardVelocity = 0;
    public Vector3 jumpDir;
    [HideInInspector] public Vector3 lastMoveDir;
    [Header("gravity Settings: ")]
    [SerializeField] private float gravity = 14.0f;
    [SerializeField] public float jumpVel = 14f;
    [SerializeField] public float slideForce = 10f;
    public Move currentMove, previousMove;
    public State previousState = new State();
    public Transform lastClimbedWall;

    public float maxDistFromGround;
    public RaycastHit frontHit;
    public bool jumping;
    public Ledge ledge;

    #endregion

    private void Awake()
    {
        virtualController = GetComponent<VirtualController>();
        currentMove = idleMove;
        idleMove.EnterState(this);
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (virtualController.JumpButtonPressed)
        {
            if (cc.isGrounded || playerState == State.WALLRUN_LEFT || playerState == State.WALLRUN_RIGHT)
            {
                if (!jumping)
                    StartCoroutine(JumpRoutine());
            }
        }

        if (!cc.isGrounded && playerState != State.WALLRUN_LEFT && playerState != State.WALLRUN_RIGHT && playerState != State.CLIMB)
            verticalDir.y -= gravity * Time.deltaTime;

        if (virtualController.VerticalMovement != 0 || virtualController.HorizontalMovement != 0)
        {
            if (playerState != State.RUN
                           && playerState != State.CLIMB
                           && playerState != State.AIRBORNE
                           && playerState != State.SLIDE)
            {
                SwitchStates(State.RUN, runMove);
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            SwitchStates(State.SLIDE, SlideMove);
        }

        else if (Input.GetKeyUp(KeyCode.Z) && playerState == State.SLIDE)
        {
            SwitchStates(State.RUN, runMove);
        }

        HandleState();
        MovePlayer();
    }

    public void CheckGrounded()
    {
        if (cc.isGrounded && playerState != State.CLIMB)
        {
            verticalDir = Vector3.zero;
        }

        if (DistToGround() > 1f && cc.isGrounded && playerState != State.CLIMB)
        {
            SwitchStates(State.AIRBORNE, airborneMove);
        }
        else
        {
            SwitchStates(State.IDLE, idleMove);
        }
    }

    private void HandleState()
    {
        switch (playerState)
        {
            case State.IDLE:
                idleMove.Act(this);
                break;
            case State.RUN:
                runMove.Act(this);
                break;
            case State.WALLRUN_LEFT:
                wallRunMoveLeft.Act(this);
                break;
            case State.WALLRUN_RIGHT:
                wallrunMoveRight.Act(this);
                break;
            case State.CLIMB:
                ClimbMove.Act(this);
                break;
            case State.SLIDE:
                SlideMove.Act(this);
                break;
            case State.AIRBORNE:
                airborneMove.Act(this);
                break;
        }
    }

    private bool ValidateStateChange(State newState, Move nextMove)
    {
        foreach (State noGoState in currentMove.noGoStates)
        {
            if (newState == playerState)
            {
                return false;
            }
            if (newState == noGoState)
            {
                return false;
            }
        }
        return true;

    }
    public void SwitchStates(State nextState, Move nextMove)
    {
        if (ValidateStateChange(nextState, nextMove))
        {
            currentMove.ExitState(this);
            currentMove = nextMove;
            currentMove.EnterState(this);
            //print("next state: " + nextState.ToString());
            playerState = nextState;
        }
        else
        {
            //print("invalid state: " + nextState.ToString());
        }
    }

    private float DistToGround()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f);
        Debug.DrawRay(transform.position, Vector3.down, Color.blue);
        return hit.distance;
    }
    public void ApplyGravity()
    {
        if (jumping)
            verticalDir.y = gravity * Time.deltaTime * 15;
    }

    public void SetInitVel()
    {
        moveDir = new Vector3(0, 1, 0);
    }
    public void MovePlayer()
    {
        if (playerState != State.CLIMB)
            moveDir.y = verticalDir.y;
        cc.Move(moveDir * Time.deltaTime);
    }

    public void Accelerate()
    {
        float rate = 0;
        if (virtualController.VerticalMovement != 0 || virtualController.VerticalMovement != 0)
        {
            rate = accelRatePerSec;
        }

        else
            rate = decelRatePerSec;

        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    public void FrontCollisionTest()
    {
        if (playerState == State.WALLRUN_LEFT || playerState == State.WALLRUN_RIGHT)
            return;

        if (virtualController.ClimbButtonPressed && playerState != State.CLIMB)
        {
            ledge = ledgeDetector.CheckLedge();
            if (!ledge.empty)
            {
                SwitchStates(State.CLIMB, ClimbMove);
            }
        }
    }

    public void LeftRightCollisionsTest()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.right, out hit, 1 + cc.skinWidth))
        {
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            if (dot == 0)
            {
                //Debug.Log("wallrun right");
                wallrunDir = Vector3.Cross(hit.normal, Vector3.up);
                Debug.DrawRay(hit.point, wallrunDir, Color.red, 10f);
                isWallrun_Right = true;
                SwitchStates(State.WALLRUN_RIGHT, wallrunMoveRight);
            }
        }

        else if (Physics.Raycast(transform.position + Vector3.up, -transform.right, out hit, 1 + cc.skinWidth))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) == 0)
            {
                //Debug.Log("left wall" + hit.transform.CompareTag("wall"));

                wallrunDir = Vector3.Cross(hit.transform.up, hit.normal);
                Debug.DrawRay(hit.point, wallrunDir, Color.red, 10f);
                isWallrun_Left = true;
                SwitchStates(State.WALLRUN_LEFT, wallRunMoveLeft);
            }
        }
        else
        {
            SwitchStates(State.RUN, runMove);
            wallrunDir = Vector3.zero;
            isWallrun_Right = false;
            isWallrun_Left = false;
        }
    }

    private IEnumerator JumpRoutine()
    {
        float jumpTime = 0.75f;
        float timer = 0;
        verticalDir = Vector3.zero;
        jumping = true;
        while (timer < jumpTime)
        {
            //Calculate how far through the jump we are as a percentage
            //apply the full jump force on the first frame, then apply less force
            //each consecutive frame
            float proportionCompleted = timer / jumpTime;

            Vector3 thisFrameJumpVector = Vector3.Lerp(jumpDir, Vector3.zero, proportionCompleted);
            verticalDir = thisFrameJumpVector;
            // moveDir.z += transform.forward.z * 10f;
            timer += Time.deltaTime;
            yield return null;
        }

        jumping = false;
    }
}