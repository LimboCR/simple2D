using UnityEngine;

public class SkillEffectApplier : MonoBehaviour
{
    public StatusEffectSO effectToApply;
    public float lifeTime = 5f;

    private void Update()
    {
        StartCoroutine(CoroutineUtils.WaitThenDo(lifeTime, () => Destroy(gameObject)));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(effectToApply != null)
        {
            if (other.TryGetComponent<StatusEffectHandler>(out StatusEffectHandler handler))
            {
                handler.ApplyEffectSO(effectToApply);
            }
        }

        Destroy(gameObject); // Optional: destroy fireball on impact
    }
}
