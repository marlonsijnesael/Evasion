using UnityEngine;
using XInputDotNetPure;
public class VirtualController : MonoBehaviour
{
    public PlayerIndex playerIndex;
    private bool playerIndexSet = false;
    private GamePadState state;
    private GamePadState prevState;
    [SerializeField] private GameObject controllerUI;
    #region Check if controller is connected
    private void FixedUpdate()
    {
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        // GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
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
                //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
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
    #endregion
    #region buttons
    public bool WallrunButtonPressed
    {
        get
        {
            return state.Triggers.Left > 0;
        }
    }
    public bool JumpButtonPressedThisFrame
    {
        get
        {
            return prevState.Buttons.A == ButtonState.Pressed;
        }
    }
    public bool JumpButtonHold
    {
        get
        {
            return prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed;
        }
    }
    public bool JumpButtonReleased
    {
        get
        {
            return prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released;
        }
    }

    public bool JumpButtonReleased
    {
        get
        {
            return prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released;
        }
    }

    public bool ClimbButtonPressed
    {
        get
        {
            return  state.Triggers.Right > 0;
            //return prevState.Buttons.A != ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed;
        }
    }
    #endregion
    #region analogstick-left
    public float HorizontalMovement
    {
        get
        {
            return state.ThumbSticks.Left.X;
        }
    }
    public float VerticalMovement
    {
        get
        {
            return state.ThumbSticks.Left.Y;
        }
    }
    #endregion
    #region  analogstick-right
    public float VerticalLook
    {
        get
        {
            return -state.ThumbSticks.Right.Y;
        }
    }
    public float HorizontalLook
    {
        get
        {
            return state.ThumbSticks.Right.X;
        }
    }
    #endregion
}