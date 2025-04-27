using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfHealingSkill", menuName = "Skills/Skill Tree/Self Heal")]
public class SelfHealingSkill : SkillBase
{
    public float ResetTime = 12f;
    public bool SkillAtCooldown = false;
    [SerializeField] private AudioClip _healingSFX;

    private Coroutine _delay = null;
    public override void Execute(GameObject user, GameObject target = null)
    {
        if (SkillAtCooldown) return;
        if (appliableEffect == null) return;

        appliableEffect.ApplyEffect(user);
        if(_healingSFX != null)

        _delay = null;
        _delay = GameManager.Instance.StartCoroutine(StartDealay());
    }

    public IEnumerator StartDealay()
    {
        SkillAtCooldown = true;

        float elapsedTime = 0f;
        while (elapsedTime < ResetTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SkillAtCooldown = false;
        yield break;
    }
}
