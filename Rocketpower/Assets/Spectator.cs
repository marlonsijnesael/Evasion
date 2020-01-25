using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{
    public Camera spectatorCam;


    public Camera currentCam;

    public static Spectator _instance;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        currentCam = spectatorCam;
    }

    private void LateUpdate()
    {
        transform.position = currentCam.transform.position;
        transform.rotation = currentCam.transform.rotation;
        spectatorCam.fieldOfView = currentCam.fieldOfView;
    }

    public void SwitchCam(Camera newCam)
    {
        currentCam = newCam;
        spectatorCam.fieldOfView = currentCam.fieldOfView;
    }
}

