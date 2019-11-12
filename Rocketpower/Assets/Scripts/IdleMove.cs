using UnityEngine;

[CreateAssetMenu(menuName = "idle")]
public class IdleMove : Move
{
    public AnimContrller.AnimationStates animation;

    public override void EnterState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), true);
    }

    public override void Act(StateMachine _owner)
    {
        _owner.SetInitVel();
        _owner.FrontCollisionTest();
        _owner.playerRotator.UpdateRotation();
    }

    public override void ExitState(StateMachine _owner)
    {
        _owner.animationController.SetBool(_owner.animator, animation.ToString(), false);
    }
}