using UnityEngine;

public class NPCHealth : Health
{
    public bool TakingDamage = false;
    public int DamageMultiplier = 0;
    private void Awake()
    {
    }

    public override void TakeDamage(int amount)
    {
        if (!alive) return;
        TakingDamage = true;
        DamageMultiplier += 1;
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            alive = false;
        }
    }
}
