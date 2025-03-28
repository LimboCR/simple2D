public class NPCIdleState : NPCState
{
    public NPCIdleState(CloseCombatNPCBase npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine) { }

    public override void AnimationTriggerEvent(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        npc.NPCIdleBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.NPCIdleBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.NPCIdleBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.NPCIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        npc.NPCIdleBaseInstance.DoPhysicsUpdateLogic();
    }
}
