using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FireballSkill", menuName = "Skills/Skill Tree/Fire Ball")]
public class FireballSkill : SkillBase
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;

    public override void Execute(GameObject user, GameObject target = null)
    {
        if (fireballPrefab == null) return;

        Vector3 newPosition = new(user.transform.position.x, user.transform.position.y + 1f, 0f);

        GameObject fireball = Instantiate(fireballPrefab, newPosition, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        Vector2 direction = (target != null)
            ? (target.transform.position - user.transform.position).normalized
            : user.transform.right;

        rb.linearVelocity = direction * fireballSpeed;

        if(appliableEffect != null && fireball.TryGetComponent<SkillEffectApplier>(out SkillEffectApplier effectApplier))
        {
            effectApplier.effectToApply = appliableEffect;
        }
    }
}
