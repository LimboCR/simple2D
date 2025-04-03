using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
    [SerializeField] private float duration = 5f;  // Default duration

    public float Duration => duration;

    public abstract void ApplyEffect(GameObject target);
}