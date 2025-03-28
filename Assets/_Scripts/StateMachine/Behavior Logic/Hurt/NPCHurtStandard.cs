using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Hurt-Standard", menuName = "NPC Logic/Standard/Hurt")]
public class NPCHurtStandard : NPCHurtSOBase
{
    [SerializeField] private string _hurtAnimationName;
    private Coroutine _delayCoroutine;
    public override void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        npc.StateLocker = true;

        npc.AnimationState.ChangeAnimationState(_hurtAnimationName);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        if (_delayCoroutine != null)
        {
            npc.StopCoroutine(_delayCoroutine); // Stop if interrupted
            _delayCoroutine = null;
        }

        npc.ActiveState = CloseCombatNPCBase.NPCStateCheck.CrossState;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (npc.GotDamagedCounter > 4) npc.GotDamagedCounter = 0;

        if (npc.AnimationState.IsAnimationFinished(npc.AnimationState.CurrentState))
            if (_delayCoroutine == null) npc.StartCoroutine(Delay());

        if(npc.StateLocker == false)
        {
            if (npc.combatNav.AbleToAttack)
                npc.StateMachine.ChangeState(npc.AttackState);
            else if (!npc.combatNav.AbleToAttack && npc.combatNav.CurrentTarget != null)
                npc.StateMachine.ChangeState(npc.ChaseState);
            else if (npc.combatNav.CurrentTarget == null)
                npc.StateMachine.ChangeState(npc.IdleState);
        }
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
        npc.TakingDamage = false;
        npc.StateLocker = false;
    }

    private IEnumerator Delay()
    {
        float delayTime = Random.Range(0.5f, 1f);
        float elapsedTime = 0f;

        while (elapsedTime < delayTime)
        {
            //if (npc.NPCHp.TakingDamage)
            //{
            //    npc.StateLocker = false;
            //    yield break;
            //}

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        npc.TakingDamage = false;
        npc.StateLocker = false;
    }
}
