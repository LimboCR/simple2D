using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalEventsManager;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if(!Application.isPlaying) return null;

            if (instance == null)
                Instantiate(Resources.Load<GameManager>("GameManager"));
#endif
            return instance;
        }
    }

    [Header("GameStates")]
    [SerializeField] private GameStates currentGameState;
    [SerializeField] private NewPlayerController _player;
    public NewPlayerController Player
    {
        get { return _player; }
        set
        {
            if (_player == null)
            {
                _player = value;
            }
        }
    }

    [SerializeField] public List<GameObject> EnemyAtScene = new();
    public List<GameObject> SpawnPoints = new();

    public static LayerMask AllInGameLayers;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        EventSubscriber();
        

        Player = FindAnyObjectByType<NewPlayerController>();
    }

    private void Update()
    {
        if (Keyboard.current.numpad0Key.wasPressedThisFrame)
        {
            SaveSystem.Save();
        }
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            SaveSystem.Load();
        }
    }

    #region Static Getters

    public GameObject GetPlayerGameObject()
    {
        if (Player != null) return Player.gameObject;
        return null;
    }

    #endregion

    #region Event Based Actions

    private void EventSubscriber()
    {
        OnStateChange.AddListener(ChangeGameState);
        OnEnemySpawn.AddListener(AddEnemyAtScene);
        OnEnemyRemove.AddListener(RemoveEnemyAtScene);
        OnSpawnAdd.AddListener(AddSpawnPointAtScene);
        OnSpawnRemove.AddListener(RemoveSpawnPointAtScene);
    }
    private void ChangeGameState(GameStates state)
    {
        currentGameState = state;
        BroadcastActualGameState(currentGameState);
    }

    private void AddEnemyAtScene(GameObject enemy)
    {
        EnemyAtScene.Add(enemy);
    }

    private void RemoveEnemyAtScene(GameObject enemy)
    {
        EnemyAtScene.Remove(enemy);
    }

    private void AddSpawnPointAtScene(GameObject spawnPoint)
    {
        SpawnPoints.Add(spawnPoint);
    }
    private void RemoveSpawnPointAtScene(GameObject spawnPoint)
    {
        SpawnPoints.Remove(spawnPoint);
    }

    #endregion
}

public enum GameStates
{
    Start,
    Pause,
    Unpause,
    Save,
    Restart,
    Exit
}
