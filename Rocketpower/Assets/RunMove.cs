using UnityEngine;

[CreateAssetMenu(menuName = "run")]
public class RunMove : Move
{
    public override void Act(StateMachine _owner)
    {
        if (!_owner.cc.isGrounded)// && !wallrunning_right)
        {
            _owner.verticalDir.y -= _owner.gravity * Time.deltaTime * 10;
        }
        _owner.moveDir += (_owner.transform.rotation * Vector3.forward);//* forwardVelocity;
    }
    public override void switchState(StateMachine _owner)
    {

    }
}
