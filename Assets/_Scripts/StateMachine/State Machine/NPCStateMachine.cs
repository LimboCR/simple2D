using UnityEngine;

public class NPCStateMachine
{
    public NPCState CurrentNPCState { get; set; }

    public void Initialize(NPCState startingState)
    {
        CurrentNPCState = startingState;
        CurrentNPCState.EnterState();
    }

    public void ChangeState(NPCState newState)
    {
        CurrentNPCState.ExitState();
        CurrentNPCState = newState;
        CurrentNPCState.EnterState();
    }
}
