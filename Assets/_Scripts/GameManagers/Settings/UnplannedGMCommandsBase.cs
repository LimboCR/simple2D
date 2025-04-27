using UnityEngine;

public abstract class UnplannedGMCommandsBase
{
    public GameManager GM;
    public UnplannedGMCommandsBase(GameManager GM)
    {
        this.GM = GM;
    }
    public virtual void DoAction() { }
    public virtual void DoUpdateAction() { }
    public virtual void DoReset() { }
    public virtual void DoOnSceneLoad() { }
    public virtual void DoOnDestroy() { }
    public virtual void DoOnApplicationQuit() { }
}
