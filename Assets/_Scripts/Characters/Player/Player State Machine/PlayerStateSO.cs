using UnityEngine;

public abstract class PlayerStateSO : ScriptableObject
{
    protected GameObject playerObj;
    protected NewPlayerController player;
    public virtual void Initialize(GameObject playerObj, NewPlayerController player)
    {
        this.playerObj = playerObj;
        this.player = player;
    }

    public virtual void DoEnterLogic() { }
    public virtual void DoExitLogic() { ResetValues(); }
    public virtual void DoFrameUpdateLogic() { }
    public virtual void DoPhysicsUpdateLogic() { }
    public virtual void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType) { }
    public virtual void ResetValues() { }
}
