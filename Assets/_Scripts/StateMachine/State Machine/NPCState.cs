using UnityEngine;

public class NPCState
{
    protected CloseCombatNPCBase npc;
    protected NPCStateMachine npcStateMachine;

    public NPCState(CloseCombatNPCBase npc, NPCStateMachine npcStateMachine)
    {
        this.npc = npc;
        this.npcStateMachine = npcStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(CloseCombatNPCBase.AnimationTriggerType triggerType) { }

}
