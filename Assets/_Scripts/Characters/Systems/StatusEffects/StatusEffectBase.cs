using UnityEngine;

public abstract class StatusEffectBase
{
    protected GameObject target;
    protected float duration;
    protected float elapsedTime = 0;

    public StatusEffectBase(GameObject target, float duration)
    {
        this.target = target;
        this.duration = duration;
    }

    public virtual void ApplyEffect() { }  // Called when effect starts
    public virtual void UpdateEffect(float deltaTime) { elapsedTime += deltaTime; }
    public virtual void RemoveEffect() { }  // Called when effect ends
    public bool IsExpired => elapsedTime >= duration;
}
