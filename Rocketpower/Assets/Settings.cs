using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings GameSettings;

    public bool invert_Y = false;
    public bool jumpOnPress = true;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (GameSettings == null)
        {
            GameSettings = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
