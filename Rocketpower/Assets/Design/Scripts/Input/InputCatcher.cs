using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCatcher : MonoBehaviour
{
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

