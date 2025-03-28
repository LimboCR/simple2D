using System.Collections;
using UnityEngine;

public class NPCIdleSOBase : ScriptableObject
{
    protected CloseCombatNPCBase npc;
    protected GameObject gameObject;

    protected Coroutine _idleCoroutine;

    public virtual void Initialize(GameObject gameObject, CloseCombatNPCBase npc)
    {
        this.gameObject = gameObject;
        this.npc = npc;
    }

    public virtual void DoEnterLogic()
    {
        npc.ActiveState = CloseCombatNPCBase.NPCStateCheck.Idle;
        npc.ReturningWaypoint = npc.NeedToReturn();
    }
    public virtual void DoExitLogic() 
    { 
        ResetValues();

        if (_idleCoroutine != null)
        {
            npc.StopCoroutine(_idleCoroutine); // Stop if interrupted
            _idleCoroutine = null;
        }
    }
    public virtual void DoFrameUpdateLogic() { }
    public virtual void DoPhysicsUpdateLogic() { }
    public virtual void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType) { }
    public virtual void ResetValues() { }

    protected virtual IEnumerator Idle()
    {
        yield break;
    }
}
