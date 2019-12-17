using UnityEngine;

[CreateAssetMenu(menuName = "wallrun")]
public class WallrunMove : Move
{
    public AnimationManager.AnimationStates animation;
    public float wallRunCoolDown = 0.01f;

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
        _owner.moveDir.y = _owner.minimumJumpVelocity * 2;
        Debug.Log("JUMP POWER = " + (power).ToString());

        _owner.SwitchStates(StateMachine.State.AIRBORNE, _owner.airborneMove);
        _owner.StartCoroutine(_owner.ClimbCooldown(_owner.canWallRun, wallRunCoolDown));

    }

    private void Move(StateMachine _owner)
    {
        _owner.stateMoveDir = (_owner.transform.rotation * Vector3.forward - _owner.wallrunDir.normalized);
    }
}