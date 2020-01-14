using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerJumpPad : MonoBehaviour
{
    RaycastHit rayJumpPad;
    bool isStartCoroutine = true;

    void Start()
    {

    }

    void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(Vector3.down), Color.red, 3);
        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.down), out rayJumpPad, 2) && rayJumpPad.transform.tag == "JumpPad")
        {
            JumpPad pad = rayJumpPad.transform.GetComponent<JumpPad>();

            if (pad != null && this.GetComponent<StateMachine>().forwardVelocity > 1)
            {
                pad.Launch(this.GetComponent<StateMachine>());
            }
        }
    }
}
