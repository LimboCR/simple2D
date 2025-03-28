using UnityEngine;

public interface IDamageble
{
    

    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
    float RegenDelay { get; set; }
    int RegenRate { get; set; }

    bool Alive { get; set; }
    bool TakingDamage { get; set; }
    int GotDamagedCounter { get; set; }

    void TakeDamage(int amount);
    void Die();
    void SetInitialIDamageble(SafeInstantiation.HealthStats? healthStats);

}
