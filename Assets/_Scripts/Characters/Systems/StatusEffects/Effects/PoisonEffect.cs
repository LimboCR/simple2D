using UnityEngine;

public class PoisonEffect : StatusEffectBase
{
    private float poisonDamage;
    private float tickInterval;
    private float timeSinceLastTick = 0;

    private bool descriptionDisplayed = false;
    private StatusEffectDescription description;
    private GameObject StatusEffectImage;

    public PoisonEffect(GameObject target, float duration, float poisonDamage, float tickInterval, StatusEffectDescription? description = null)
        : base(target, duration)
    {
        this.poisonDamage = poisonDamage;
        this.tickInterval = tickInterval;
        this.description = description;
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
        if (descriptionDisplayed)
        {
            InGameUIManager.Instance.RemoveStatusEffectImage(StatusEffectImage);
            descriptionDisplayed = false;
        }
    }

    public override void UpdateEffect(float deltaTime)
    {
        base.UpdateEffect(deltaTime);
        if (target.CompareTag("Player") && description != null && !descriptionDisplayed)
        {
            descriptionDisplayed = true;
            StatusEffectImage = InGameUIManager.Instance.DisplayStatusEffect(description);
        }

        timeSinceLastTick += deltaTime;

        if (timeSinceLastTick >= tickInterval)
        {
            target.GetComponent<IDamageble>()?.TakeDamage(poisonDamage);
            timeSinceLastTick = 0;
        }
    }
}
