using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Standard", menuName = "NPC Logic/Standard/Chase")]
public class NPCChaseStandard : NPCChaseSOBase
{
    [SerializeField] private string _jumpAnimationName;
    [SerializeField] private string _chaseAnimationName;
    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        npc.IgnoreWaypoints = true;
        npc.IsAgrresive = true;
        npc.AnimationState.ChangeAnimationState(_chaseAnimationName);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        npc.IsAgrresive = true;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (npc.combatNav.CurrentTarget == null) npc.StateMachine.ChangeState(npc.IdleState); // Stop chasing if unreachable

        if (npc.combatNav.AbleToAttack)
            npc.StateMachine.ChangeState(npc.AttackState);

        Chase();
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
        
    }

    protected override void Chase()
    {
        if (npc.combatNav.CurrentTarget == null) return;
        npc.NPCMove(npc.RunSpeed);

        if (npc.combatNav.CurrentTarget.gameObject.transform.position.x > npc.transform.position.x && !npc.LookingRight)
            npc.FlipSides(npc.LookingRight);
        else if(npc.combatNav.CurrentTarget.gameObject.transform.position.x < npc.transform.position.x && npc.LookingRight)
            npc.FlipSides(npc.LookingRight);

        npc.GroundCheck();

        if (npc.IsJumping) npc.AnimationState.ChangeAnimationState(_jumpAnimationName);
        else if (!npc.IsJumping && npc.AnimationState.CurrentState == _jumpAnimationName)
            npc.AnimationState.ChangeAnimationState(_chaseAnimationName);
    }
}
