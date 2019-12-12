using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XInputDotNetPure;
public class VirtualController : MonoBehaviour
{
    public PlayerIndex playerIndex;
    private bool playerIndexSet = false;
    private GamePadState state;
    private GamePadState prevState;

    public List<ButtonState> pressList_A = new List<ButtonState>();
    private float timeHold_Button_A = 0f;

    [SerializeField] private GameObject controllerUI;
    #region Check if controller is connected


    private void Awake()
    {
        for (int i = 0; i < 5; i++)
            pressList_A.Add(prevState.Buttons.A);
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

    private void FixedUpdate()
    {
        pressList_A.RemoveAt(4);
        pressList_A.Insert(0, state.Buttons.A);

        string list = "";
        foreach (ButtonState bState in pressList_A)
            list += " " + bState.ToString();

    }


    private void Update()
    {

        if (JumpButtonHold && (VerticalMovement != 0 || HorizontalMovement != 0))
        {
            timeHold_Button_A += Time.deltaTime * 5;
            Debug.Log("jumppower: " + timeHold_Button_A);
        }
        else
        {
            if (timeHold_Button_A > 0)
                timeHold_Button_A = -Time.deltaTime;
            else
                timeHold_Button_A = 0;
        }
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

    public float Time_Hold_Button_A
    {
        get
        {
            return timeHold_Button_A;
        }
    }

    public bool JumpButtonPressedThisFrame
    {
        get
        {
            return prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed;
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

    public bool ClimbButtonPressed
    {
        get
        {
            return state.Triggers.Right > 0;
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