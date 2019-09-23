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
        float moveHorizontal = InputCatcher.GetAxis("horizontal", playerID);
        float moveVertical = InputCatcher.GetAxis("vertical", playerID);
        Vector3 pos =transform.position ;
        pos.x += moveHorizontal;
        pos.z += moveVertical;

        transform.Translate(transform.forward* Time.deltaTime * moveVertical * speed, Space.World);
    }
}
