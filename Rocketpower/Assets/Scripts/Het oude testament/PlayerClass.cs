using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerClass : MonoBehaviour
{
    [SerializeField] private int playerID;
    [SerializeField] private PlayerMove playerMovement;
    [SerializeField] private CameraController cameraMovement;

    private void FixedUpdate()
    {
        playerMovement.Move(playerID);
    }

}
