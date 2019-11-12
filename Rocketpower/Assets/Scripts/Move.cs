using UnityEngine;

[System.Serializable]
public abstract class Move : ScriptableObject
{
    public StateMachine.State[] noGoStates;

    public abstract void EnterState(StateMachine _owner);

    public abstract void Act(StateMachine _owner);

    public abstract void ExitState(StateMachine _owner);

}
