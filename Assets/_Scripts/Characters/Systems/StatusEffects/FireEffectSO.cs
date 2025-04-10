using UnityEngine;

[CreateAssetMenu(fileName = "FireEffect", menuName = "Status Effects/Legacy/Fire")]
public class FireEffectSO : StatusEffectSO
{
    [SerializeField] private float tickDamage = 5f;
    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private StatusEffectDescription description;

    public override void ApplyEffect(GameObject target)
    {
        target.GetComponent<StatusEffectHandler>().AddEffect(new FireEffect(target, Duration, tickDamage, tickInterval, description));
    }
}