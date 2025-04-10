using UnityEngine;

[CreateAssetMenu(fileName = "HelingEffect", menuName = "Status Effects/Legacy/Heal")]
public class HealingEffectSO : StatusEffectSO
{
    [SerializeField] private float tickHeal = 5f;
    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private StatusEffectDescription description;

    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<StatusEffectHandler>().AddEffect(new HealingEffect(target, Duration, tickHeal, tickInterval, description));
    }
}
