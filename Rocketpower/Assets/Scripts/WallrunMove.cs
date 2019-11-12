using UnityEngine;

[CreateAssetMenu(menuName = "wallrun")]
public class WallrunMove : Move
{
    public AnimContrller.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {
        //  _owner.verticalDir = Vector3.zero;
        _owner.LeftRightCollisionsTest();
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
        _owner.wallrunDir = Vector3.zero;
    }

    private void Move(StateMachine _owner)
    {
        _owner.lastMoveDir.y = 0f;
        _owner.moveDir = _owner.lastMoveDir + (_owner.transform.rotation * Vector3.forward - _owner.wallrunDir.normalized);
    }
}
