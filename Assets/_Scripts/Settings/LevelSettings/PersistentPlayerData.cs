using UnityEngine;

[CreateAssetMenu(fileName = "PersistentPlayerData", menuName = "Game/Persistent Player Data")]
public class PersistentPlayerData : ScriptableObject
{
    public float Health;
    public float MaxHealth;
    public Vector3 LastKnownPosition;

    public void SaveFromPlayer(NewPlayerController player)
    {
        Health = player.CurrentHealth;
        MaxHealth = player.MaxHealth;
        LastKnownPosition = player.transform.position;
    }

    public void LoadToPlayer(NewPlayerController player)
    {
        player.CurrentHealth = Health;
        player.MaxHealth = MaxHealth;
        player.transform.position = LastKnownPosition;
    }
}