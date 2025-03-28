public class NPCRunState : NPCState
{
    public NPCRunState(CloseCombatNPCBase npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine) { }

    public override void AnimationTriggerEvent(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        npc.NPCRunBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.NPCRunBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.NPCRunBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.NPCRunBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        npc.NPCRunBaseInstance.DoPhysicsUpdateLogic();
    }
}
