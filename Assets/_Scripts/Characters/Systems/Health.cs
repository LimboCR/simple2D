using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Character Stats")]
    [Tooltip("Current health of the character")] public int currentHealth;
    [Tooltip("Max health of this character")] public int maxHealth;
    [Tooltip("Max health of this character")] public float regenDelay;
    [Tooltip("Max health of this character")] public int regenRate;
    [Tooltip("Verifies if Charachter is alive")] public bool alive;
    [ColorUsage(true)] public Color colorFull, colorDamaged, colorHalf, colorLow;

    public virtual void TakeDamage(int amount) { }
}
