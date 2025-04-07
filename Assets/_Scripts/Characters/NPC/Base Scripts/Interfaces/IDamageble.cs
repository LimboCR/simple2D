using UnityEngine;

public interface IDamageble
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    float RegenDelay { get; set; }
    float RegenRate { get; set; }

    bool Alive { get; set; }
    bool TakingDamage { get; set; }
    int GotDamagedCounter { get; set; }

    void TakeDamage(float amount);
    void TakeHealing(float amount);
    void Die();
    void SetInitialIDamageble(SafeInstantiation.HealthStats? healthStats);
}
