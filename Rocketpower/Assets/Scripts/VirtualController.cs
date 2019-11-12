using UnityEngine;

public class VirtualController : MonoBehaviour
{
    public string jumpButton;
    public string horizontalAxis;
    public string verticalAxis;
    public string prefix;
    public string climbButton;

    public bool JumpButtonPressed
    {
        get
        {
            return Input.GetButtonDown(jumpButton);
        }
    }

    public float HorizontalMovement
    {
        get
        {
            return Input.GetAxis(horizontalAxis);
        }
    }

    public bool ClimbButtonPressed
    {
        get
        {
            Debug.Log(Input.GetButtonDown(climbButton));
            return Input.GetButtonDown(climbButton);
        }
    }
    public float VerticalMovement
    {
        get
        {
            return Input.GetAxis(verticalAxis);
        }
    }

    public static bool GetButton(string _button, int _controllerID)
    {
        string checkButton = "J" + _controllerID + _button;
        return Input.GetButtonDown(checkButton);
    }

    public static float GetAxis(string _axis, int _controllerID)
    {
        string checkAxis = "J" + _controllerID + _axis;
        return Input.GetAxis(checkAxis);
    }

}
