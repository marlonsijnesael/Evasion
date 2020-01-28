using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using XInputDotNetPure;
public class VirtualController : MonoBehaviour
{
    public PlayerIndex playerIndex;
    private bool playerIndexSet = false;
    private GamePadState state;
    private GamePadState prevState;
    private float timeHold_Button_A = 0f;

    [SerializeField] private GameObject controllerUI;
    #region Check if controller is connected


    public int inputQueueSize = 5;
    public LimitedQueue<bool> inputQueue;

    private void Awake()
    {
        inputQueue = new LimitedQueue<bool>(inputQueueSize);
    }

    public bool GetNextKey()
    {
        if (inputQueue.Count > 0)
            return inputQueue.Dequeue();
        else
            return false;
    }

    public void SetVibration()
    {
        StartCoroutine(Vibrate());
    }

    private IEnumerator Vibrate()
    {
        GamePad.SetVibration(playerIndex, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    public void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        PlayerIndex testPlayerIndex = playerIndex;
        GamePadState testState = GamePad.GetState(testPlayerIndex);
        if (testState.IsConnected)
        {
            if (!playerIndexSet || !prevState.IsConnected)
            {
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

        if (JumpButtonPressedThisFrame)
        {
            inputQueue.Enqueue(JumpButtonPressedThisFrame);
            Debug.Log(inputQueue.Count);

        }

        if (JumpButtonHold && (VerticalMovement != 0 || HorizontalMovement != 0))
        {
            timeHold_Button_A += Time.deltaTime * 5;
        }
        else
        {
            if (timeHold_Button_A > 1)
                timeHold_Button_A -= Time.deltaTime;
            else
                timeHold_Button_A = 1;
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

    public bool StartPressed
    {
        get
        {
            return prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed;
        }
    }

    public float Time_Hold_Button_A
    {
        get
        {
            return timeHold_Button_A;
        }
        set
        {
            timeHold_Button_A = value;
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
            return Settings.GameSettings.invert_Y ? -state.ThumbSticks.Right.Y : -state.ThumbSticks.Right.Y;
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



[System.Serializable]
public class LimitedQueue<T> : Queue<T>
{
    public int Limit { get; set; }

    public LimitedQueue(int limit) : base(limit)
    {
        Limit = limit;
    }

    public new void Enqueue(T item)
    {
        while (Count >= Limit)
        {
            Dequeue();
        }
        base.Enqueue(item);
    }
}