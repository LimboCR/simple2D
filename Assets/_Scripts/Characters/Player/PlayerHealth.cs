using UnityEngine;

public class PlayerHealth : Health
{
    public override void TakeDamage(int amount)
    {
        if (!alive) return;

        NewPlayerController playerController = this.gameObject.GetComponent<NewPlayerController>();
        playerController.StateMachine.ChangeState(playerController.HurtState);

        currentHealth -= amount;

        GlobalEventsManager.SendPlayerCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            alive = false;
        }
    }

    
}
