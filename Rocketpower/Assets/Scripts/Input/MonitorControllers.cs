using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorControllers : MonoBehaviour
{
    private string[] joystickNames;
    private bool AllControllerConnected = false;
    private int playerCount = 4;

    public delegate void ControllerCallback(string _popUp, bool _setActive, float _timeScale);
    public static event ControllerCallback OnConnect;

    private void Start()
    {
        joystickNames = new string[Input.GetJoystickNames().Length];
        StartCoroutine(CheckControllers());
    }

    //called when controller is connected
    private void OnControllerConnected(string _name, int _id)
    {
        OnConnect("connect", false, 1);
        Debug.Log(Time.time + " Controller " + _id + " is connected using: " + _name);
    }

    //called when controller is disconnected
    private void OnControllerDisconnected(string _name, int _id)
    {
        AllControllerConnected = false;
        OnConnect("connect",true , 1);
        Debug.LogWarning(" Controller " + _id + " is disconnected."+ _name);
    }

    private IEnumerator CheckControllers()
    {
        string[] currentJoysticks = Input.GetJoystickNames();
        if (currentJoysticks.Length > 0)
        {
            for (int i = 0; i < currentJoysticks.Length; ++i)
            {
                if (!string.IsNullOrEmpty(currentJoysticks[i]) && currentJoysticks[i] != joystickNames[i])
                {
                    OnControllerConnected(currentJoysticks[i], i);
                }
                else if (string.IsNullOrEmpty(currentJoysticks[i]) && currentJoysticks[i] != joystickNames[i])
                {
                    OnControllerDisconnected(currentJoysticks[i], i);
                }
            }
        }
        yield return null;
        joystickNames = currentJoysticks;
        StartCoroutine(CheckControllers());
    }




    



}
