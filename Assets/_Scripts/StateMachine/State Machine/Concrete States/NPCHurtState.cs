public class NPCHurtState : NPCState
{
    public NPCHurtState(CloseCombatNPCBase npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine) { }

    public override void AnimationTriggerEvent(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        npc.NPCHurtBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.NPCHurtBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.NPCHurtBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.NPCHurtBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        npc.NPCHurtBaseInstance.DoPhysicsUpdateLogic();
    }
}
