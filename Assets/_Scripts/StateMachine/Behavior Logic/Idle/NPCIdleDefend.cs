using System.Collections;
using UnityEngine;

public class NPCIdleDefend : NPCIdleSOBase
{
    [SerializeField] private string _aggroedIdleAnimName;
    [SerializeField] private Transform _defencivePosition;
    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        if (_defencivePosition == null) _defencivePosition = npc.transform;
        npc.AnimationState.ChangeAnimationState(_aggroedIdleAnimName);

        if (_idleCoroutine == null)
            _idleCoroutine = npc.StartCoroutine(Idle());
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
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

    protected override IEnumerator Idle()
    {
        while (npc.combatNav.CurrentTarget == null)
        {
            if (npc.combatNav.CurrentTarget != null)
            {
                npc.StateMachine.ChangeState(npc.ChaseState);
                yield break;
            }
            yield return null;
        }
        yield break;
    }
}
