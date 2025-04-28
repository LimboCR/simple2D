using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="FireballSkill", menuName = "Skills/Skill Tree/Fire Ball")]
public class FireballSkill : SkillBase
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    [SerializeField] private AudioClip _fireBallSFX;

    public float ResetTime = 8f;
    public bool SkillAtCooldown = false;
    private Coroutine _delay = null;

    public override void Execute(GameObject user, GameObject target = null)
    {
        if (SkillAtCooldown) return;
        if (fireballPrefab == null) return;

        Vector3 newPosition = new(user.transform.position.x, user.transform.position.y + 1f, 0f);

        GameObject fireball = Instantiate(fireballPrefab, newPosition, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        if(_fireBallSFX != null) AudioManager.PlayerInterSourcePlay(_fireBallSFX, PlayMode.force);

        Vector2 direction = (target != null)
            ? (target.transform.position - user.transform.position).normalized
            : user.transform.right;

        rb.linearVelocity = direction * fireballSpeed;

        if(appliableEffect != null && fireball.TryGetComponent<SkillEffectApplier>(out SkillEffectApplier effectApplier))
        {
            effectApplier.effectToApply = appliableEffect;
        }

        _delay = null;
        _delay = GameManager.Instance.StartCoroutine(StartDealay());
    }

    public IEnumerator StartDealay()
    {
        SkillAtCooldown = true;

        float elapsedTime = 0f;
        while(elapsedTime < ResetTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SkillAtCooldown = false;
        yield break;
    }
}