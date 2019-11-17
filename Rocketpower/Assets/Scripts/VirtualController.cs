using UnityEngine;
using XInputDotNetPure; // Required in C#

public class VirtualController : MonoBehaviour
{
    bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public GameObject controllerUI;


    private void FixedUpdate()
    {
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
    }

    // Update is called once per frame
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

    public bool JumpButtonPressed
    {
        get
        {
            return prevState.Buttons.A != ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed;// && cc.isGrounded;
        }
    }

    public float HorizontalMovement
    {
        get
        {
            return state.ThumbSticks.Left.X;
        }
    }

    public bool ClimbButtonPressed
    {
        get
        {
            Debug.Log(state.Triggers.Right > 0);
            return state.Triggers.Right > 0;
        }
    }
    public float VerticalMovement
    {
        get
        {
            return state.ThumbSticks.Left.Y;
        }
    }
    public float VerticalLook
    {
        get
        {
            return state.ThumbSticks.Right.Y;
        }
    }
    public float HorizontalLook
    {
        get
        {
            return state.ThumbSticks.Right.X;
        }
    }
}
