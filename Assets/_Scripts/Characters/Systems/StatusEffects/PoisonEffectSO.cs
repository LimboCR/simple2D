using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEffect", menuName = "Status Effects/Poison")]
public class PoisonEffectSO : StatusEffectSO
{
    [SerializeField] private float poisonDamage = 3f;
    [SerializeField] private float tickInterval = 2f;

    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<StatusEffectHandler>().AddEffect(new PoisonEffect(target, Duration, poisonDamage, tickInterval));
    }
}