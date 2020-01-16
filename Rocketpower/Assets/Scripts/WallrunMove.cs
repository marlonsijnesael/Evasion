using UnityEngine;

[CreateAssetMenu(menuName = "wallrun")]
public class WallrunMove : Move
{
    public AnimationManager.AnimationStates animation;
    public float coolDownTime = 0.2f;
    public float sideJumpMultiplier = 5;

    public override void EnterState(StateMachine _owner)
    {
        // _owner.jumpVec.y = _owner.wallrunDir.y;
        _owner.jumpVec = Vector3.zero;
        _owner.moveDir.y = 0;
        _owner.gravity = _owner.gravity * 0.5f;
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
        _owner.jumpVec = _owner.wallrunSide * sideJumpMultiplier * _owner.jumpMultiplier;
        _owner.jumpVec.y = _owner.minimumJumpVelocity * _owner.jumpMultiplier;

        _owner.StartCoroutine(_owner.WallrunCoolDown(coolDownTime));


    }

    private void Move(StateMachine _owner)
    {
        RaycastHit hit;
        if (Physics.Raycast(_owner.transform.position + Vector3.up, _owner.transform.forward, out hit, 1))
        {
            if (hit.transform.GetComponent<Renderer>().bounds.max.y > _owner.cc.height)
                _owner.jumpVec.y = -2;
            else
            {
                _owner.jumpVec.y = 2;
            }
        }
        _owner.stateMoveDir = (_owner.transform.rotation * Vector3.forward - _owner.wallrunDir.normalized);
    }
}