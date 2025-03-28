public class NPCChaseState : NPCState
{
    public NPCChaseState(CloseCombatNPCBase npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine) { }

    public override void AnimationTriggerEvent(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        npc.NPCChaseBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.NPCChaseBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.NPCChaseBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.NPCChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        npc.NPCChaseBaseInstance.DoPhysicsUpdateLogic();
    }
}
