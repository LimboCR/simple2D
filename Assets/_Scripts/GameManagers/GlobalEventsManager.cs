using System;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventsManager: MonoBehaviour
{
    public static UnityEvent<int> OnEnemyKilled = new();

    public static UnityEvent<float> OnPlayerHealthChanged = new();
    public static UnityEvent<float> OnPlayerTakeDamage = new();
    public static UnityEvent<int> OnPlayerCoinsChanged = new();

    public static UnityEvent<GameStates> OnStateChange = new();
    public static UnityEvent<GameStates> GameStateListener = new();

    public static UnityEvent<GameObject> OnEnemySpawn = new();
    public static UnityEvent<GameObject> OnEnemyRemove = new();

    public static UnityEvent<GameObject> OnSpawnAdd = new();
    public static UnityEvent<GameObject> OnSpawnRemove = new();

    public static void SendEnemyKilled(int remainingCount)
    {
        OnEnemyKilled.Invoke(remainingCount);
    }

    public static void SendPlayerHealthChanged(float currentHealth)
    {
        OnPlayerHealthChanged.Invoke(currentHealth);
    }

    public static void SendPlayerTookDamage(float amount)
    {
        OnPlayerTakeDamage.Invoke(amount);
    }

    public static void SendCoinsChanged(int currentCoins)
    {
        OnPlayerCoinsChanged.Invoke(currentCoins);
    }

    public static void SendNewGameState(GameStates state)
    {
        OnStateChange.Invoke(state);
    }

    public static void BroadcastActualGameState(GameStates state)
    {
        GameStateListener.Invoke(state);
    }

    public static void SendEnemySpawn(GameObject enemy)
    {
        OnEnemySpawn.Invoke(enemy);
    }

    public static void SendEnemyRemove(GameObject enemy)
    {
        OnEnemyRemove.Invoke(enemy);
    }

    public static void SendSpawnPoint(GameObject spawnPoint)
    {
        OnSpawnAdd.Invoke(spawnPoint);
    }

    public static void RemoveSpawnPoint(GameObject spawnPoint)
    {
        OnSpawnRemove.Invoke(spawnPoint);
    }
}
