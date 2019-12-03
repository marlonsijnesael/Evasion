using UnityEngine;

[CreateAssetMenu(menuName = "run")]
public class RunMove : Move
{
    public AnimationManager.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {
        _owner.SetInitVel();
        _owner.Accelerate();
        _owner.FrontCollisionTest();
        _owner.LeftRightCollisionsTest();
        _owner.playerRotator.UpdateRotation();
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }
    public override void Jump(StateMachine _owner, float power)
    {
        if (_owner.isGrounded)
        {
            _owner.isGrounded = false;
            _owner.moveDir.y = _owner.jumpVelocity + power;
            _owner.stateMoveDir += (_owner.transform.rotation * Vector3.forward) * (_owner.forwardJumpMultiplier * power - _owner.GetAngle());
        }
    }

    private void Move(StateMachine _owner)
    {
        _owner.stateMoveDir = (_owner.transform.rotation * Vector3.forward) * _owner.forwardVelocity;
        _owner.lastMoveDir = _owner.moveDir;
    }
}
