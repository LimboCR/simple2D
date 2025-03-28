public class NPCReturnState : NPCState
{
    public NPCReturnState(CloseCombatNPCBase npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine) { }

    public override void AnimationTriggerEvent(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        npc.NPCReturnBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.NPCReturnBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.NPCReturnBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.NPCReturnBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        npc.NPCReturnBaseInstance.DoPhysicsUpdateLogic();
    }
}
