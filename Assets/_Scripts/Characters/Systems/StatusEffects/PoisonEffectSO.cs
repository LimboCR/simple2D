using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEffect", menuName = "Status Effects/Legacy/Poison")]
public class PoisonEffectSO : StatusEffectSO
{
    [SerializeField] private float poisonDamage = 3f;
    [SerializeField] private float tickInterval = 2f;
    [SerializeField] private StatusEffectDescription description;

    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<StatusEffectHandler>().AddEffect(new PoisonEffect(target, Duration, poisonDamage, tickInterval, description));
    }
}