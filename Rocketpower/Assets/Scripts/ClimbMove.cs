
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "wallClimb")]
public class ClimbMove : Move
{

    public float climbSpeed = 2.5f;
    public AnimationManager.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
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
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }

    private void Move(StateMachine _owner)
    {
        Vector3 yPosOrigin = _owner.ledge.playerPosition;
        yPosOrigin.y = _owner.ledge.hitPosition.y;
        Vector3 climbDir = _owner.ledge.hitPosition - _owner.ledge.playerPosition;

        if (_owner.transform.position.y < yPosOrigin.y && _owner.virtualController.ClimbButtonPressed)
        {
            Debug.Log("climbig");
            _owner.moveDir.y = climbDir.y + 1;
            //_owner.stateMoveDir = climbDir + Vector3.up + (_owner.transform.rotation * Vector3.forward) * _owner.forwardVelocity;
        }

        else
        {
            Debug.Log("stop climbing");
            _owner.SwitchStates(StateMachine.State.AIRBORNE, _owner.airborneMove);
        }
        Debug.DrawRay(_owner.ledge.playerPosition, climbDir, Color.red, 10f);
        // newPos.y += _owner.ledge.height;
        // _owner.transform.position = newPos;

    }
}
