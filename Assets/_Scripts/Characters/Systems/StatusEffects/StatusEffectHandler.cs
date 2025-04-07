using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    [SerializeField] private List<StatusEffectBase> activeEffects = new();

    public bool ApplyFireEffect = false;
    public bool ApplyPoisonEffect = false;

    [SerializeField] private StatusEffectSO effectToApply;

    public void AddEffect(StatusEffectBase effect)
    {
        activeEffects.Add(effect);
        effect.ApplyEffect();
    }

    public void ApplyEffectSO(StatusEffectSO effectSO)
    {
        effectSO.ApplyEffect(gameObject);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (ApplyFireEffect)
        {
            ApplyFireEffect = false;
            ApplyEffectSO(effectToApply);
        }

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].UpdateEffect(deltaTime);

            if (activeEffects[i].IsExpired)
            {
                activeEffects[i].RemoveEffect();
                activeEffects.RemoveAt(i);
            }
        }
    }
}
