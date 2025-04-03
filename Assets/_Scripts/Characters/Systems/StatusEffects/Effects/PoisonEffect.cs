using UnityEngine;

public class PoisonEffect : StatusEffectBase
{
    private float poisonDamage;
    private float tickInterval;
    private float timeSinceLastTick = 0;

    public PoisonEffect(GameObject target, float duration, float poisonDamage, float tickInterval)
        : base(target, duration)
    {
        this.poisonDamage = poisonDamage;
        this.tickInterval = tickInterval;
    }

    public override void UpdateEffect(float deltaTime)
    {
        base.UpdateEffect(deltaTime);
        timeSinceLastTick += deltaTime;

        if (timeSinceLastTick >= tickInterval)
        {
            target.GetComponent<IDamageble>()?.TakeDamage(poisonDamage);
            timeSinceLastTick = 0;
        }
    }
}
