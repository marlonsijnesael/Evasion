using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    public float JumpPadHeight;
    public float JumpPadVelocity;

    public void Launch(StateMachine owner)
    {
        owner.gameObject.GetComponent<StateMachine>().jumpMultiplier = JumpPadHeight;
        owner.gameObject.GetComponent<StateMachine>().forwardJumpMultiplier = JumpPadVelocity;
        owner.gameObject.GetComponent<StateMachine>().Jump();
        owner.gameObject.GetComponent<StateMachine>().jumpMultiplier = 1.19f;
        owner.gameObject.GetComponent<StateMachine>().forwardJumpMultiplier = 2.3f;

    }

}
