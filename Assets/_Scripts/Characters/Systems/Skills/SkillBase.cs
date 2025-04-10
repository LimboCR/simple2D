using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public string ActionName;
    public StatusEffectSO appliableEffect;

    public abstract void Execute(GameObject user, GameObject target = null);
}