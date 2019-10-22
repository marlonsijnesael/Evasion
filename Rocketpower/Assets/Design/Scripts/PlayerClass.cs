using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    
    [SerializeField] private int playerID;
    [SerializeField] private Camera playerCam;
    private float speed = 5;
    float moveHorizontal = 0f;
    private void Awake()
    {
     //  Display.displays[playerID].Activate();
    }

    private void Start()
    {
       // playerCam.targetDisplay = playerID;

    }

    private void Update()
    {
        moveHorizontal += InputCatcher.GetAxis("horizontal", playerID);
        float moveVertical = InputCatcher.GetAxis("vertical", playerID);
        Vector3 newPosition = new Vector3 (moveHorizontal, 0.0f, moveVertical) + transform.forward;
        Vector3 moveForward = transform.forward * moveVertical;
        Vector3 rotateForward = moveForward;
        rotateForward.x += moveHorizontal;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateForward), Time.deltaTime
            );
        transform.Translate(moveForward * speed * Time.deltaTime, Space.World);

    }
}
