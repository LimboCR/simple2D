using UnityEngine;

[CreateAssetMenu(fileName = "Run-Standard", menuName = "NPC Logic/Standard/Run")]
public class NPCRunStandard : NPCRunSOBase
{
    [SerializeField] private string _jumpAnimationName;
    [SerializeField] protected string _runAnimName;
    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        npc.IgnoreWaypoints = false;
        npc.AnimationState.ChangeAnimationState(_runAnimName);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        npc.GroundCheck();

        npc.IsHitObstacle = npc.ObstacleCheck();
        if (npc.IsHitObstacle && !npc.IsJumping)
            npc.StateMachine.ChangeState(npc.IdleState);

        npc.NPCMove(npc.RunSpeed);

        if (npc.IsJumping) npc.AnimationState.ChangeAnimationState(_jumpAnimationName);
        else if (!npc.IsJumping && npc.AnimationState.CurrentState == _jumpAnimationName)
            npc.AnimationState.ChangeAnimationState(_runAnimName);

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
