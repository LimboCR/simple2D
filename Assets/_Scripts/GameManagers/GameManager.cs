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
    [SerializeField] public static GameStates currentGameState;
    [SerializeField] private static NewPlayerController _player;
    public static NewPlayerController Player
    {
        get { return _player; }
        set
        {
            if (_player == null)
            {
                _player = value;
            }
            else Debug.LogError($"Player's Script Already found: {Player.gameObject.name}");
        }
    }

    [SerializeField] public List<GameObject> EnemyAtScene = new();
    public List<GameObject> SpawnPoints = new();

    public List<string> LayerMasksToRemoveFromEnemy;
    public List<string> LayerMasksToRemoveFromFriendly;

    public static LayerMask AllInGameLayers;

    //Saving data
    public static string SaveFileName = null;
    public static string SaveFileFolder = "SaveFiles";
    public static string QuickSaveFolder = "QuickSaves";
    public static int QuickSaveCounter = 0;

    private void Awake()
    {
        AllInGameLayers = LayerMask.GetMask(LayerMasksToRemoveFromEnemy.ToString());
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

    private static void PerformQuickSave()
    {
        if(QuickSaveCounter == 5) QuickSaveCounter = 0;
        SaveFileName = $"QuickSave{QuickSaveCounter}";
        SaveFileFolder = MultiSaveSystem.MSS.GetSavePath(SaveFileName, QuickSaveFolder);
        QuickSaveCounter++;

        currentGameState = GameStates.QuickSave;
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    private static void PerformSave(string saveFileName)
    {
        currentGameState = GameStates.QuickSave;
        SaveFileFolder = MultiSaveSystem.MSS.GetSavePath(saveFileName, SaveFileFolder);
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    private static void PerformQuickLoad()
    {
        currentGameState = GameStates.Restart;
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    public static void PerformLoading()
    {
        currentGameState = GameStates.Load;
        SaveFileName = $"QuickSave{QuickSaveCounter}";
        SaveFileFolder = MultiSaveSystem.MSS.GetSavePath(SaveFileName, QuickSaveFolder);
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }
}

public enum GameStates
{
    Start,
    Play,
    Pause,
    Unpause,
    Save,
    QuickSave,
    Restart,
    Load,
    Exit
}
