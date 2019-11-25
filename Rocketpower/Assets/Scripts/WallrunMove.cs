using UnityEngine;

[CreateAssetMenu(menuName = "wallrun")]
public class WallrunMove : Move
{
    public AnimationManager.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.cameraScript.locked = true;
        _owner.playerScript.locked = true;
        _owner.moveDir.y = 0f;
        _owner.gravity *= 1.7f;
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {

        _owner.LeftRightCollisionsTest();
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.cameraScript.locked = false;
        _owner.playerScript.locked = false;
        _owner.gravity = _owner.normalGravity;
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
        _owner.wallrunDir = Vector3.zero;
    }

    private void Move(StateMachine _owner)
    {

        _owner.stateMoveDir = (_owner.transform.rotation * Vector3.forward - _owner.wallrunDir.normalized);
    }
}
