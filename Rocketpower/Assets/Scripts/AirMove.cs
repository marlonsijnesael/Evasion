﻿using UnityEngine;

[CreateAssetMenu(menuName = "air")]
public class AirMove : Move
{
    public AnimationManager.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }

    public override void Act(StateMachine _owner)
    {
        _owner.playerRotator.UpdateRotation();
        _owner.LeftRightCollisionsTest();
        _owner.FrontCollisionTest();
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    private void Move(StateMachine _owner)
    {
        _owner.stateMoveDir = (_owner.transform.rotation * Vector3.forward) * _owner.forwardVelocity;
        // _owner.verticalDir = Vector3.zero;
    }
}

