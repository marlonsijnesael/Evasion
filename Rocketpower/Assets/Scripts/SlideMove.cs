using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "slide")]
public class SlideMove : Move
{
    public AnimContrller.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {
        _owner.ApplyGravity();
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }

    private void Move(StateMachine _owner)
    {
        _owner.moveDir = Vector3.zero;
        Vector3 force = Vector3.zero;

        _owner.moveDir += (_owner.transform.rotation * Vector3.forward) * _owner.slideForce; //groundChecker.groundSlopeDir.normalized;

    }
}