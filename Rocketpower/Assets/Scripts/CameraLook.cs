﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    //Make sure you have a camera, it will determine the direction the character faces
    public Transform cam;

    float speed = 10f;    //How fast the player can move
    float turnSpeed = 100;    //How fast the player can rotate
    Animator animator;//You may not need an animator, but if so declare it here
    public GameObject player;
    public VirtualController virtualController;
    private void Awake()
    {
        virtualController = player.GetComponent<VirtualController>();
    }

    public void UpdateRotation()
    {
        float hLook = virtualController.HorizontalMovement;
        float vLook = virtualController.VerticalMovement;

        //right is shorthand for (1,0,0) or the x value            forward is short for (0,0,1) or the z value 
        Vector3 dir = (cam.right * hLook) + (cam.forward * vLook);

        dir.y = 0;//Keeps character upright against slight fluctuations

        if (hLook != 0 || vLook != 0)
        {
            //rotate from this /........to this............../.........at this speed 
            player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        }
    }
}