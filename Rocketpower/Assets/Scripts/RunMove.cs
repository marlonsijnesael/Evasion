using UnityEngine;

[CreateAssetMenu(menuName = "run")]
public class RunMove : Move
{
    public AnimationManager.AnimationStates animation;
    public override void EnterState(StateMachine _owner)
    {
        //_owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {
        _owner.SetInitVel();
        _owner.Accelerate();
        _owner.FrontCollisionTest();
        _owner.LeftRightCollisionsTest();
        //_owner.playerRotator.UpdateRotation();
        if (_owner.isGrounded)
            _owner.moveDir.y = 0;
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        //_owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }
    public override void Jump(StateMachine _owner, float power)
    {
        if (_owner.isGrounded)
        {
            _owner.isGrounded = false;
            _owner.moveDir.y = _owner.minimumJumpVelocity * _owner.jumpMultiplier + power;
            Debug.Log("JUMP POWER = " + (_owner.moveDir.y).ToString());
			_owner.audioObj.GetComponent<GeneralAudio>().JumpSound();
		
            _owner.stateMoveDir += (_owner.transform.forward + _owner.transform.up) * (1 + (_owner.GetAngle() / 100) * (_owner.forwardJumpMultiplier * power));//s
        }
    }

    private void Move(StateMachine _owner)
    {
        _owner.stateMoveDir = _owner.transform.forward * _owner.forwardVelocity;
        _owner.lastMoveDir = _owner.moveDir;
    }
}