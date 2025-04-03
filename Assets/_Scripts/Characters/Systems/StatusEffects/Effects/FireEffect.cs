using UnityEngine;

public class FireEffect : StatusEffectBase
{
    private float tickDamage;
    private float tickInterval;
    private float timeSinceLastTick = 0;

    public FireEffect(GameObject target, float duration, float tickDamage, float tickInterval)
        : base(target, duration)
    {
        this.tickDamage = tickDamage;
        this.tickInterval = tickInterval;
    }

    public override void UpdateEffect(float deltaTime)
    {
        base.UpdateEffect(deltaTime);
        timeSinceLastTick += deltaTime;

        if (timeSinceLastTick >= tickInterval)
        {
            target.GetComponent<IDamageble>()?.TakeDamage(tickDamage);
            timeSinceLastTick = 0;
        }
    }
}
