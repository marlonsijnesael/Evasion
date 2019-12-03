using UnityEngine;

[CreateAssetMenu(menuName = "wallrun")]
public class WallrunMove : Move
{
    public AnimationManager.AnimationStates animation;


    public override void EnterState(StateMachine _owner)
    {
        _owner.moveDir.y = _owner.wallrunDir.y;
        _owner.gravity *= 1.7f;
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {
        _owner.playerRotator.UpdateRotation();
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.gravity = _owner.normalGravity;
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
        _owner.wallrunDir = Vector3.zero;
    }

    public override void Jump(StateMachine _owner, float power)
    {
        _owner.moveDir.y = _owner.jumpVelocity;

    }

    private void Move(StateMachine _owner)
    {
        _owner.stateMoveDir = (_owner.transform.rotation * Vector3.forward - _owner.wallrunDir.normalized);
    }
}
