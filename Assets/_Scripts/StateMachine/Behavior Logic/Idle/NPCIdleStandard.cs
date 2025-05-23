using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Standard", menuName = "NPC Logic/Standard/IdleStanding")]
public class NPCIdleStandard : NPCIdleSOBase
{
    [SerializeField] private string _standardIdleAnimName;
    [SerializeField] private string _aggroedIdleAnimName;

    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        if (!npc.IsAgrresive) npc.AnimationState.ChangeAnimationState(_standardIdleAnimName);
        else npc.AnimationState.ChangeAnimationState(_aggroedIdleAnimName);

        npc.ReturningWaypoint = npc.NeedToReturn();

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
        float idleTime = Random.Range(2f, 7f);
        float elapsedTime = 0f;

        while (elapsedTime < idleTime)
        {
            if (npc.combatNav.CurrentTarget != null)
            {
                npc.StateMachine.ChangeState(npc.ChaseState);
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (npc.IsHitObstacle)
        {
            npc.IsHitObstacle = false;
            npc.FlipSides(npc.LookingRight);
        }

        if (npc.IdlingAtWaypoint)
        {
            npc.IdlingAtWaypoint = false;
            npc.FlipSides(npc.LookingRight);
        }

        if (npc.ReturningWaypoint != null)
            npc.StateMachine.ChangeState(npc.ReturnState);

        if (!npc.IsAgrresive)
            npc.StateMachine.ChangeState(npc.WalkState);
        else if (npc.IsAgrresive)
            npc.StateMachine.ChangeState(npc.RunState);
    }
}
