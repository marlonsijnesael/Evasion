using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "slide")]
public class SlideMove : Move
{
    public AnimationManager.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }

    private void Move(StateMachine _owner)
    {
        // _owner.moveDir = Vector3.zero;
        Vector3 force = Vector3.zero;
        _owner.stateMoveDir += (_owner.transform.rotation * Vector3.forward) * _owner.slideForce;
    }
}