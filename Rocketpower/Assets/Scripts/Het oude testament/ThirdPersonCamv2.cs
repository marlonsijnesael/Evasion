using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamv2 : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;
    private void FixedUpdate()
    {
        transform.position = player.position + offset;
        float rotationSpeed = 25.0f;
        float rotation = Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }



}
