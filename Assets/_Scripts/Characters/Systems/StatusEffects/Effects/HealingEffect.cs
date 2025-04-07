using UnityEngine;

public class HealingEffect : StatusEffectBase
{
    private float tickHeal;
    private float tickInterval;
    private float timeSinceLastTick = 0;

    public HealingEffect(GameObject target, float duration, float tickHeal, float tickInterval)
        : base(target, duration)
    {
        this.tickHeal = tickHeal;
        this.tickInterval = tickInterval;
    }

    public override void UpdateEffect(float deltaTime)
    {
        base.UpdateEffect(deltaTime);
        timeSinceLastTick += deltaTime;

        if (timeSinceLastTick >= tickInterval)
        {
            target.GetComponent<IDamageble>()?.TakeHealing(tickHeal);
            timeSinceLastTick = 0;
        }
    }
}
