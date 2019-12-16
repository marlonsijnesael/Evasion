using UnityEngine;

[CreateAssetMenu(menuName = "idle")]
public class IdleMove : Move
{
    public AnimationManager.AnimationStates animation;
    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }
    public override void Act(StateMachine _owner)
    {
        _owner.SetInitVel();
        _owner.FrontCollisionTest();
        //_owner.playerRotator.UpdateRotation();
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
            _owner.moveDir.y = _owner.minimumJumpVelocity;
        }
    }

}