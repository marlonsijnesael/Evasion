using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Move runMove, wallrunMove, ClimbMove, SlideMove;
    public enum State { IDLE, RUN, WALLRUN_RIGHT, WALLRUN_LEFT, CLIMB, SLIDE, AIRBORNE }
    public State playerState = new State();
    public CharacterController cc;

    public Vector3 verticalDir = Vector3.zero;
    public Vector3 initVelocity = new Vector3(0, -1, 0);
    public Vector3 moveDir = Vector3.zero;
    public float gravity = 14f;

    private void Update()
    {
        SetInitVel();
        CheckGrounded();
        FSM();
        MovePlayer();
    }


    private void CheckGrounded()
    {
        if (!cc.isGrounded)
        {
            // verticalDir.y -= gravity * Time.deltaTime;
        }
    }

    private void FSM()
    {
        switch (playerState)
        {
            case State.IDLE:
                break;
            case State.RUN:
                runMove.Act(this);
                break;
            case State.WALLRUN_LEFT:
                wallrunMove.Act(this);
                break;
            case State.WALLRUN_RIGHT:
                wallrunMove.Act(this);
                break;
            case State.CLIMB:
                ClimbMove.Act(this);
                break;
            case State.SLIDE:
                SlideMove.Act(this);
                break;
            case State.AIRBORNE:
                break;
        }
    }

    private void SetInitVel()
    {
        moveDir = initVelocity;
    }
    public void MovePlayer()
    {
        cc.Move((moveDir + verticalDir) * Time.deltaTime);
    }

}
