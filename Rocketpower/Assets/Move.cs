using UnityEngine;

public abstract class Move : ScriptableObject
{
    public abstract void Act(StateMachine _owner);

    public abstract void switchState(StateMachine _owner);

}
