using UnityEngine;

[CreateAssetMenu(menuName = "air")]
public class AirMove : Move
{
    public AnimationManager.AnimationStates animation;
    public float timeFallingToRoll = 2f;
    
    public override void EnterState(StateMachine _owner)
    {
        _owner.timeFalling = 0;
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }

    public override void Act(StateMachine _owner)
    {
        _owner.LeftRightCollisionsTest();
        _owner.FrontCollisionTest();
        _owner.playerRotator.UpdateRotation();
        _owner.timeFalling += Time.deltaTime;
        Debug.Log("falling: " + _owner.timeFalling);
        Move(_owner);
    }

    public override void ExitState(StateMachine _owner)
    {
        if(_owner.timeFalling >= timeFallingToRoll)
        {

        }
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }
    public override void Jump(StateMachine _owner, float power)
    {

    }

 
    
    private void Move(StateMachine _owner)
    {
        _owner.stateMoveDir = (_owner.transform.rotation * Vector3.forward) * _owner.forwardVelocity;
        // _owner.verticalDir = Vector3.zero;
    }
}
