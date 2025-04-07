using UnityEngine;

public class FireEffect : StatusEffectBase
{
    private float tickDamage;
    private float tickInterval;
    private float timeSinceLastTick = 0;

    private bool descriptionDisplayed = false;
    private StatusEffectDescription description;
    private GameObject StatusEffectImage;

    public FireEffect(GameObject target, float duration, float tickDamage, float tickInterval, StatusEffectDescription? description = null)
        : base(target, duration)
    {
        this.tickDamage = tickDamage;
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
        if(target.CompareTag("Player") && description != null && !descriptionDisplayed)
        {
            descriptionDisplayed = true;
            StatusEffectImage = InGameUIManager.Instance.DisplayStatusEffect(description);
        }

        timeSinceLastTick += deltaTime;

        if (timeSinceLastTick >= tickInterval)
        {
            target.GetComponent<IDamageble>()?.TakeDamage(tickDamage);
            timeSinceLastTick = 0;
        }
    }
}
