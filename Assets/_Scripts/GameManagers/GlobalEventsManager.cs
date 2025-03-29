using System;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventsManager: MonoBehaviour
{
    public static UnityEvent<int> OnEnemyKilled = new UnityEvent<int>();

    public static UnityEvent<int> OnPlayerHealthChanged = new UnityEvent<int>();
    public static UnityEvent<int> OnPlayerCoinsChanged = new UnityEvent<int>();

    public static UnityEvent<GameStates> OnStateChange = new UnityEvent<GameStates>();
    public static UnityEvent<GameStates> GameStateListener = new UnityEvent<GameStates>();

    public static UnityEvent<GameObject> OnEnemySpawn = new();
    public static UnityEvent<GameObject> OnEnemyRemove = new();

    public static UnityEvent<GameObject> OnSpawnAdd = new();
    public static UnityEvent<GameObject> OnSpawnRemove = new();

    public static void SendEnemyKilled(int remainingCount)
    {
        OnEnemyKilled.Invoke(remainingCount);
    }

    public static void SendPlayerHealthChanged(int currentHealth)
    {
        OnPlayerHealthChanged.Invoke(currentHealth);
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
