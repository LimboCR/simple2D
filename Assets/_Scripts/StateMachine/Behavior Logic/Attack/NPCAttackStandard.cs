using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Standard", menuName = "NPC Logic/Standard/Attack")]
public class NPCAttackStandard : NPCAttackSOBase
{
    [SerializeField] private string _attackAnimationName;
    [SerializeField] private int _damageAmount;
    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        if(triggerType == CloseCombatNPCBase.AnimationTriggerType.DamageTargetsInHitZone)
        {
            DamageTargetsInHitZone();
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        npc.IgnoreWaypoints = true;
        npc.AnimationState.ChangeAnimationState(_attackAnimationName);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        npc.IsAgrresive = true;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (npc.combatNav.CurrentTarget == null)
            npc.StateMachine.ChangeState(npc.IdleState);

        if (npc.combatNav.CurrentTarget != null && !npc.combatNav.AbleToAttack)
            npc.StateMachine.ChangeState(npc.ChaseState);

        if (npc.combatNav.CurrentTarget != null && npc.combatNav.CurrentTarget.gameObject.transform.position.x > npc.transform.position.x && !npc.LookingRight)
            npc.FlipSides(npc.LookingRight);
        else if (npc.combatNav.CurrentTarget != null && npc.combatNav.CurrentTarget.gameObject.transform.position.x < npc.transform.position.x && npc.LookingRight)
            npc.FlipSides(npc.LookingRight);
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

    private void DamageTargetsInHitZone()
    {
        if(npc.combatNav.TargetsInHitZone != null && npc.combatNav.TargetsInHitZone.Count > 0)
        {
            foreach (Collider2D target in npc.combatNav.TargetsInHitZone)
            {
                if (target.TryGetComponent<IDamageble>(out IDamageble npc))
                {
                    npc.TakeDamage(_damageAmount);
                    //Debug.Log($"|AttackLog| {gameObject.name} inflicted {_damageAmount} damage to {target.name}");
                }
                    
            }
        }
    }
}
