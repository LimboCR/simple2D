using UnityEngine;

public class PlayerState
{
    protected NewPlayerController player;
    protected PlayerStateMachine playerStateMachine;

    public PlayerState(NewPlayerController player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(CloseCombatNPCBase.AnimationTriggerType triggerType) { }
}
