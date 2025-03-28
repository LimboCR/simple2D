using UnityEngine;

[CreateAssetMenu(fileName = "Walk-Standard", menuName = "NPC Logic/Standard/Walk")]
public class NPCWalkStandard : NPCWalkSOBase
{
    [SerializeField] private string _jumpAnimationName;
    [SerializeField] private string _walkAnimName;
    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        npc.IgnoreWaypoints = false;
        npc.AnimationState.ChangeAnimationState(_walkAnimName);
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

        if (npc.IsJumping) npc.AnimationState.ChangeAnimationState(_jumpAnimationName);
        else if (!npc.IsJumping && npc.AnimationState.CurrentState == _jumpAnimationName)
            npc.AnimationState.ChangeAnimationState(_walkAnimName);

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
    }
}
