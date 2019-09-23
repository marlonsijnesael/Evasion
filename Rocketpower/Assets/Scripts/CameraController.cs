using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lookSensitivty = 1;
    public float lookSmoothDamp = 1;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float yRot = 0f;
    private float yRotV, xRotV;

    private void Update()
    {
        var angH = Input.GetAxis("J1horizontal") * 60;
        var angV = Input.GetAxis("rightstickvertical") * 45;
        transform.localEulerAngles = new Vector3(angV, angH, 0);
    }
}
