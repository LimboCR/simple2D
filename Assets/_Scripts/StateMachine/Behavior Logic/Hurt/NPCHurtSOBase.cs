using UnityEngine;

public class NPCHurtSOBase : ScriptableObject
{
    protected CloseCombatNPCBase npc;
    protected GameObject gameObject;

    public virtual void Initialize(GameObject gameObject, CloseCombatNPCBase npc)
    {
        this.gameObject = gameObject;
        this.npc = npc;
    }

    public virtual void DoEnterLogic()
    {
        npc.ActiveState = CloseCombatNPCBase.NPCStateCheck.Hurt;
    }
    public virtual void DoExitLogic()
    {
        ResetValues();
    }
    public virtual void DoFrameUpdateLogic() { }
    public virtual void DoPhysicsUpdateLogic() { }
    public virtual void DoAnimationTriggerEventLogic(CloseCombatNPCBase.AnimationTriggerType triggerType) { }
    public virtual void ResetValues() { }
}
