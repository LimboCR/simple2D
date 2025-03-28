using UnityEngine;

[CreateAssetMenu(fileName = "ReturnToBase-Standard", menuName = "NPC Logic/Standard/ReturnToBase")]
public class NPCReturnStandard : NPCReturnSOBase
{
    [SerializeField] private string _returnAnimName;
    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        npc.IgnoreWaypoints = false;
        npc.AnimationState.ChangeAnimationState(_returnAnimName);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        npc.GroundCheck();
        npc.NPCMove(npc.WalkSpeed);

        if (npc.combatNav.CurrentTarget != null)
            npc.StateMachine.ChangeState(npc.ChaseState);
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void Initialize(GameObject gameObject, CloseCombatNPCBase npc)
    {
        base.Initialize(gameObject, npc);
    }

    public override void ResetValues()
    {
        base.ResetValues();
        npc.ReturningWaypoint = null;
    }
}
