using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    public float JumpPadVelocity;

    public void Launch(StateMachine owner)
    {
        owner.gameObject.GetComponent<StateMachine>().jumpMultiplier = JumpPadVelocity;
        owner.gameObject.GetComponent<StateMachine>().Jump();
        owner.gameObject.GetComponent<StateMachine>().jumpMultiplier = 1.19f;
    }

    public IEnumerator StartJump()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

}
