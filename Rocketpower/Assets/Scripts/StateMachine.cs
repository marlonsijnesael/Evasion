using UnityEngine;

public class StateMachine : MonoBehaviour
{
    #region components


    [Header("Components: ")]
    public CharacterController cc;
    public LedgeDetection ledgeDetector;
    public RotatePlayer playerRotator;
    public AnimationManager animationController;
    public Animator animator;
    public VirtualController virtualController;
    #endregion

    #region state stuff
    [Header("States: ")]
    public Move currentMove;
    public Move previousMove;
    public Move idleMove, runMove, wallrunMoveRight, wallRunMoveLeft, ClimbMove, SlideMove, airborneMove;
    public enum State { IDLE, RUN, WALLRUN_RIGHT, WALLRUN_LEFT, CLIMB, SLIDE, AIRBORNE }
    public State playerState = new State();

    #endregion

    #region directional vectors
    [HideInInspector] public Vector3 verticalDir = Vector3.zero;
    [HideInInspector] public Vector3 wallrunDir;
    [HideInInspector] public Vector3 stateMoveDir;
    [HideInInspector] public Vector3 moveDir = Vector3.zero;
    [HideInInspector] public Vector3 lastMoveDir;
    #endregion

    #region animator booleans
    [HideInInspector] public bool isWallrun_Right;
    [HideInInspector] public bool isWallrun_Left;
    #endregion

    #region movement settings
    [Header("Movement Settings: ")]
    public float maxSpeed = 5f;
    [SerializeField] public float slideForce = 10f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    public float forwardVelocity = 0;
    #endregion

    #region gravity and jump settings
    [Header("gravity and jump Settings: ")]
    [SerializeField] private float jumpHeight = 4;
    [SerializeField] public float timeToJumpApex = .4f;
    private float gravity = 14.0f;
    private float jumpVelocity;
    [SerializeField] private float forwardJumpMultiplier = 5;
    private bool isGrounded;
    public Ledge ledge;
    #endregion

    private void Awake()
    {
        virtualController = GetComponent<VirtualController>();
        currentMove = idleMove;
        idleMove.EnterState(this);
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;


        print("gravity: " + gravity + " jump vel: " + jumpVelocity);
    }

    private void FixedUpdate()
    {
        //Put it back to Awake
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        CheckGrounded();
        SetInitVel();

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

    /// <summary>
    /// Executes the the act() function of the state Move
    /// </summary>
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

    /// <summary>
    /// Checks if the given statechange is valid
    /// by looping through the nogo states of the proposed state
    /// </summary>
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

    /// <summary>
    /// If next state is valid -> transition to new state
    /// </summary>
    public void SwitchStates(State nextState, Move nextMove)
    {
        if (ValidateStateChange(nextState, nextMove))
        {
            currentMove.ExitState(this);
            currentMove = nextMove;
            currentMove.EnterState(this);
            playerState = nextState;
        }
    }

    /// <summary>
    /// applies a little bit of gravity each frame to get an accurate 
    /// update on cc.isgrounded
    /// </summary>
    public void SetInitVel()
    {
        stateMoveDir = new Vector3(0, 001f, 0);
    }

    public void MovePlayer()
    {
        if (!cc.isGrounded)
            moveDir.y += gravity * Time.deltaTime;
        else
        {
            moveDir.y = 0;
        }
        Jump();
        moveDir.x = stateMoveDir.x;
        moveDir.z = stateMoveDir.z;
        cc.Move(moveDir * Time.deltaTime);
    }

    /// <summary>
    /// checks input from the analogstick 
    /// forward velocity will increase/decrease depending on wether the input is higher or lower than zero 
    /// </summary>
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

    private void Jump()
    {
        if (virtualController.JumpButtonPressed && isGrounded)
        {
            isGrounded = false;
            moveDir.y = jumpVelocity;
            if (virtualController.VerticalMovement != 0)
                stateMoveDir += transform.forward * forwardJumpMultiplier;
            
        }
    }

    /// <summary>
    /// Checks for collision with ground underneath player
    /// if ground is not detected, switch to idle state
    /// </summary>
    public void CheckGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up, -transform.up, out hit);
        float distToGround = hit.distance - 1;
        if (distToGround > 1f && playerState != State.CLIMB)
        {
            isGrounded = false;
            SwitchStates(State.AIRBORNE, airborneMove);
        }
        else
        {
            isGrounded = true;
            SwitchStates(State.IDLE, idleMove);
        }
    }

    /// <summary>
    /// Checks for collision with walls in front of player
    /// if wall is detected, switch to climbing state
    /// </summary>
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

    /// <summary>
    /// Checks for collision with walls left and right of player
    /// if wall is detected, switch to wallrun state
    /// </summary>
    public void LeftRightCollisionsTest()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.right, out hit, 1 + cc.skinWidth))
        {
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            if (dot == 0)
            {
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
}