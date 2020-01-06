using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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

    public Move idleMove, runMove, wallrunMoveRight, wallRunMoveLeft, ClimbMove, SlideMove, airborneMove;
    public enum State { IDLE, RUN, WALLRUN_RIGHT, WALLRUN_LEFT, CLIMB, SLIDE, AIRBORNE }
    public State playerState = new State();
    #endregion
    #region directional vectors

    [HideInInspector] public Vector3 wallrunDir;
    [HideInInspector] public Vector3 stateMoveDir = Vector3.zero;
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

    /// <summary>
    /// Jump = minimumJumpVelocity * jumpMultiplier + jumpBoost
    /// </summary>
    [Header("gravity and jump Settings: ")]
    public float gravityMultiplier = 1f; //use this to increase gravity after the calculation
    public float jumpMultiplier = 1f;
    public float maxJumpBoost = 0;
    public float jumpHeight = 4;
    [HideInInspector] public float timeToJumpApex = 0;
    [HideInInspector] public float gravity = 0;
    [HideInInspector] public float normalGravity = 0f;

    [HideInInspector] public float minimumJumpVelocity;
    [HideInInspector] public float forwardJumpMultiplier = 5;
    public bool isGrounded;

    public Ledge ledge;
    public State prevState;
    #endregion

    public Slider jumpSlider;
    public int groundedBufferSize = 5;
    public List<bool> groundedBuffer = new List<bool>();

    public bool canClimb = true;
    public float timeClimbing = 0;
    public float maxTmeClimbing = 5f;

    public bool canWallRun = true;
    public Camera playerCam;
    public float timeFalling = 0;
    public float deltaFalling = 5;
    public bool wasJump = false;

    public float FOVIdle = 60;
    public float FOVRun = 75;

    public float boostedJumpPower = 1;
    private float YpositionOnJump = 0f;

    private bool IsMoving()
    {
        return virtualController.VerticalMovement != 0 || virtualController.HorizontalMovement != 0;
    }

    private void Awake()
    {
        virtualController = GetComponent<VirtualController>();

        currentMove = idleMove;
        idleMove.EnterState(this);
        normalGravity = gravity;

        SpeedCalc();
        JumpCalc();
    }

    private void Start()
    {
        for (int i = 0; i < groundedBufferSize; i++)
        {
            groundedBuffer.Add(isGrounded);
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        SetInitVel();
        CheckInput();
        HandleState();
    }

    private void LateUpdate()
    {
        MovePlayer();
    }


    private void CheckInput()
    {
        if (IsMoving())
        {
            if (playerState != State.RUN
                           && playerState != State.CLIMB
                           && playerState != State.AIRBORNE
                           && playerState != State.SLIDE)
            {
                animationController.SetBool(animator, "running", true);
                SwitchStates(State.RUN, runMove);
            }
        }
        else if (!IsMoving())
        {
            SwitchStates(State.IDLE, idleMove);
            animationController.SetBool(animator, "running", false);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            SwitchStates(State.SLIDE, SlideMove);
        }
        else if (Input.GetKeyUp(KeyCode.Z) && playerState == State.SLIDE)
        {
            SwitchStates(State.RUN, runMove);
        }
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
            prevState = playerState;
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
        moveDir.x = 0;
        moveDir.z = 0;

    }

    private void JumpCalc()
    {
        timeToJumpApex = jumpHeight / 10;
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        minimumJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    private void SpeedCalc()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    public void MovePlayer()
    {
        StoreGroundedThisFrame();
        if (!virtualController.GetNextKey() && cc.collisionFlags.HasFlag(CollisionFlags.Above) || !WasGroundedInBuffer() && playerState != State.CLIMB && playerState != State.WALLRUN_LEFT && playerState != State.WALLRUN_LEFT)  //|| playerState == State.WALLRUN_RIGHT || playerState == State.WALLRUN_LEFT)
            moveDir.y += (gravity * gravityMultiplier) * Time.fixedDeltaTime;

        if (jumpPressed())
            Jump();

        moveDir.x = stateMoveDir.x;
        moveDir.z = stateMoveDir.z;
        cc.Move(moveDir * Time.fixedDeltaTime);
    }

    /// <summary>
    /// checks input from the analogstick
    /// forward velocity will increase/decrease depending on wether the input is higher or lower than zero
    /// </summary>
    public void Accelerate()
    {
        float rate = 0;
        if (virtualController.VerticalMovement != 0 || virtualController.HorizontalMovement != 0)
        {
            rate = accelRatePerSec;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, FOVRun, Time.deltaTime * 2);
        }
        else
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, FOVIdle, Time.deltaTime * 2);
            rate = decelRatePerSec;
        }
        forwardVelocity += rate * Time.fixedDeltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    private void StoreGroundedThisFrame()
    {
        groundedBuffer.RemoveAt(groundedBufferSize - 1);
        groundedBuffer.Insert(0, isGrounded);

        string list = "";
        foreach (bool g in groundedBuffer)
            list += " " + g.ToString();

    }

    public void ClearGroundedBuffer()
    {
        for (int i = 0; i < groundedBufferSize; i++)
        {
            groundedBuffer[i] = false;
        }
    }

    public float GetAngle()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up, -transform.up, out hit))
        {
            Vector3 tmp = Vector3.Cross(hit.normal, Vector3.down);
            Vector3 dir = Vector3.Cross(tmp, hit.normal);
            Debug.Log("angle is = " + Vector3.Angle(hit.normal, Vector3.up));
            return Vector3.Angle(hit.normal, Vector3.up);
        }
        return 1;
    }

    private bool WasGroundedInBuffer()
    {
        return groundedBuffer.Contains(true);
    }

    private IEnumerator OnJump()
    {
        ClearGroundedBuffer();
        YpositionOnJump = transform.position.y + jumpHeight;
        yield return new WaitUntil(() => groundedBuffer.Contains(true));

        //Debug.Log("landed, calling onland()");
        //StartCoroutine(OnLand(YpositionOnJump, transform.position.y));
    }

    private IEnumerator OnLand(float jumpPosY, float landingPosY)
    {
        Debug.Log(jumpPosY - landingPosY);
        animator.SetBool("B_IsRoling", false);

        if (jumpPosY - landingPosY > deltaFalling && forwardVelocity > 0)
        {
            Debug.Log("rolling");
            animator.SetBool("B_IsGrounded", true);
            animator.SetTrigger("roll");

        }
        yield return new WaitForEndOfFrame();

        YpositionOnJump = 0;
        //animator.SetBool("B_IsRoling", false);
    }

    bool jumpPressed() => Settings.GameSettings.jumpOnPress ? virtualController.JumpButtonPressedThisFrame : virtualController.JumpButtonReleased;
    private void Jump()
    {
        jumpSlider.value = virtualController.Time_Hold_Button_A / jumpSlider.maxValue;
        boostedJumpPower = 1;



        StartCoroutine(OnJump());
        wasJump = true;

        boostedJumpPower = virtualController.Time_Hold_Button_A;
        if (boostedJumpPower > maxJumpBoost)
            boostedJumpPower = maxJumpBoost;

        if (Settings.GameSettings.jumpOnPress)
        {
            boostedJumpPower = (1 + (GetAngle() / 100));
        }
        currentMove.Jump(this, boostedJumpPower);
        virtualController.Time_Hold_Button_A = 1f;
    }

    /// <summary>
    /// Checks for collision with ground underneath player
    /// if ground is not detected, switch to idle state
    /// </summary>
    public void CheckGrounded()
    {
        if (playerState != State.CLIMB)
        {
            RaycastHit hit;
            Vector3 rayPos = transform.position + Vector3.up;

            if (cc.collisionFlags.HasFlag(CollisionFlags.CollidedBelow))
            {
                isGrounded = true;
                SwitchStates(State.IDLE, idleMove);
                animationController.SetBool(this.animator, "grounded", true);
                if (wasJump)
                {
                    wasJump = false;
                    StartCoroutine(OnLand(YpositionOnJump, transform.position.y));
                }
            }

            if (Physics.Raycast(rayPos, Vector3.down, out hit))  /*1 + (jumpHeight / 2))*/
            {
                if (rayPos.y - hit.point.y > 1.1f)
                {
                    isGrounded = false;
                    animationController.SetBool(this.animator, "grounded", false);
                    SwitchStates(State.AIRBORNE, airborneMove);
                }
                else
                {
                    isGrounded = true;
                    SwitchStates(State.IDLE, idleMove);
                    animationController.SetBool(this.animator, "grounded", true);
                    if (wasJump)
                    {
                        wasJump = false;
                        StartCoroutine(OnLand(YpositionOnJump, transform.position.y));
                    }

                }
            }
            else
            {
                isGrounded = false;
                animationController.SetBool(this.animator, "grounded", false);
                SwitchStates(State.AIRBORNE, airborneMove);
            }
        }
        if ((virtualController.GetNextKey() && !isGrounded))
        {
            Jump();
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
        if (virtualController.ClimbButtonPressed && playerState != State.CLIMB && canClimb)
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
        if (Physics.Raycast(transform.position + Vector3.up, transform.right, out hit, 1 + cc.skinWidth) && virtualController.WallrunButtonPressed && canWallRun)
        {
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            if (dot > -0.7f && dot < 0.7f)
            {
                wallrunDir = Vector3.Cross(hit.normal, Vector3.up);
                wallrunDir.y *= dot;
                Debug.DrawRay(hit.point, wallrunDir, Color.red, 40f);
                isWallrun_Right = true;
                SwitchStates(State.WALLRUN_RIGHT, wallrunMoveRight);
            }
        }
        else if (Physics.Raycast(transform.position + Vector3.up, -transform.right, out hit, 1 + cc.skinWidth) && virtualController.WallrunButtonPressed && canWallRun)
        {
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            if (dot > -0.7f && dot < 0.7f)
            {
                wallrunDir = Vector3.Cross(hit.transform.up, hit.normal);
                wallrunDir.y *= dot;
                Debug.DrawRay(hit.point, wallrunDir, Color.red, 40f);
                isWallrun_Left = true;
                SwitchStates(State.WALLRUN_LEFT, wallRunMoveLeft);
            }
        }
        else
        {
            if (playerState == State.WALLRUN_LEFT || playerState == State.WALLRUN_RIGHT)
            {
                SwitchStates(State.RUN, runMove);
                wallrunDir = Vector3.zero;
                stateMoveDir = Vector3.zero;
                isWallrun_Right = false;
                isWallrun_Left = false;
            }
        }
    }

    public IEnumerator ClimbCooldown(bool cooldownState, float coolDown)
    {
        cooldownState = false;
        yield return new WaitUntil(() => groundedBuffer.Contains(true));
        cooldownState = true;
        timeClimbing = 0;
    }

}