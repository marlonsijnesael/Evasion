using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MiniControllerToTest : MonoBehaviour
{
    bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public Vector3 jumpDir;
    public GameObject controllerUI;

    private CharacterController cc;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }


    public void ControlledUpdate()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        PlayerIndex testPlayerIndex = playerIndex;
        GamePadState testState = GamePad.GetState(testPlayerIndex);
        if (testState.IsConnected)
        {
            if (!playerIndexSet || !prevState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIndex = testPlayerIndex;
                playerIndexSet = true;
            }
            if (controllerUI.activeInHierarchy)
                controllerUI.SetActive(false);
        }
        else
        {
            playerIndexSet = false;
            controllerUI.SetActive(true);
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }

    private Vector3 verticalDir = Vector3.up * 10;
    private bool jumping;
    private Vector3 moveDir = Vector3.zero;

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float gravity;
    float jumpVelocity;
    Vector3 finalVel;

    private void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("gravity: " + gravity + " jump vel: " + jumpVelocity);
    }

    private void Update()
    {
        ControlledUpdate();
        // moveDir += new Vector3(0, -1, 0);

        if (prevState.Buttons.A != ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed && cc.isGrounded)
        {
            moveDir.y = jumpVelocity;
        }
        moveDir.y += gravity * Time.deltaTime;

        cc.Move(moveDir * Time.deltaTime);

    }


}
