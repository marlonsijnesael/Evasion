using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "wallClimb")]
public class ClimbMove : Move
{
    public float climbSpeed = 2.5f;
    public float climbCoolDown = 2f;
    public float maxClimbHeight = 25f;
    public float maxClimbTime = 2f;
    public AnimationManager.AnimationStates animation;


    public bool canClimb = false;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
        _owner.jumpVec = Vector3.zero;
    }
    public override void Act(StateMachine _owner)
    {
        if (Physics.Linecast(_owner.transform.position + Vector3.up, _owner.transform.position + Vector3.up))
        {
            _owner.SwitchStates(StateMachine.State.AIRBORNE, _owner.airborneMove);
        }
        //owner.SetInitVel();
        Move(_owner);
    }

    public override void Jump(StateMachine _owner, float power)
    {

    }


    public override void ExitState(StateMachine _owner)
    {

    }

    private void Move(StateMachine _owner)
    {
        Vector3 yPosOrigin = _owner.ledge.playerPosition;
        yPosOrigin.y = _owner.ledge.hitPosition.y;
        Vector3 climbDir = _owner.ledge.hitPosition - _owner.ledge.playerPosition;
        Debug.Log("climbig " + climbDir + " y: " + yPosOrigin);


        if (_owner.transform.position.y < maxClimbHeight && _owner.transform.position.y < yPosOrigin.y && _owner.virtualController.ClimbButtonPressed && !_owner.virtualController.WallrunButtonPressed && _owner.timeClimbing < maxClimbTime)
        {
            // _owner.jumpVec.y = climbDir.y + climbSpeed;
            _owner.moveDir = Vector3.zero;
            _owner.moveDir.y = climbSpeed;
            _owner.timeClimbing += Time.deltaTime;
            //_owner.stateMoveDir = climbDir + Vector3.up + (_owner.transform.rotation * Vector3.forward) * _owner.forwardVelocity;
        }

        else
        {
            Debug.Log("stop climbing");
            _owner.jumpVec = Vector3.zero;
            _owner.SwitchStates(StateMachine.State.AIRBORNE, _owner.airborneMove);
            _owner.StartCoroutine(_owner.ClimbCooldown(_owner.canClimb, climbCoolDown));
            _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
            _owner.ClearGroundedBuffer();

        }
        // Debug.DrawRay(_owner.ledge.playerPosition, climbDir, Color.red, 10f);
    }

}