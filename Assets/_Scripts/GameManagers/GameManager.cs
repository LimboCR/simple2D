using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalEventsManager;
using MultiSaveSystem;

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
    [Space]
    [Header("Save things")]
    public static string SaveFileName;
    public static string SaveFolderName;
    public static string SaveFilePath;

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
        if (Player != null) Debug.Log("Player found");
        
    }

    private void Update()
    {
        if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
        {
            SetPath("TestSaveFolder2", "SaveBro2");
            SavePlayerDataAtPath();
        }

        if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
        {
            SetPath("TestSaveFolder", "SaveBro1");
            SavePlayerData();
        }

        if (Keyboard.current.numpad7Key.wasPressedThisFrame)
        {
            SetPath("TestSaveFolder2", "SaveBro2");
            LoadPlayerData();
        }

        if (Keyboard.current.numpad9Key.wasPressedThisFrame)
        {
            SetPath("TestSaveFolder", "SaveBro1");
            LoadPlayerData();
        }

        if (Keyboard.current.numpadMinusKey.wasPressedThisFrame)
        {
            MSSTest.SaveSession.SetActiveSlot(1);
            TestSave(MSSTest.SaveSession.ActiveSavePath);
        }
        if (Keyboard.current.numpadPlusKey.wasPressedThisFrame)
        {
            MSSTest.SaveSession.SetActiveSlot(1);
            TestSave(MSSTest.SaveSession.ActiveSavePath);
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

    public static void PerformQuickSave()
    {
        currentGameState = GameStates.QuickSave;
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    public static void PerformSave(string saveFileName)
    {
        currentGameState = GameStates.QuickSave;
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    public static void PerformQuickLoad()
    {

        currentGameState = GameStates.Restart;
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    public static void PerformLoading()
    {
        currentGameState = GameStates.Load;
        BroadcastActualGameState(currentGameState);

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    private static void SetupSaving()
    {
        //int quickSavesCount = MSS.GetAllSaveFiles(QuickSaveFolder)
    }

    #region Saving
    public void TestSave(string path)
    {
        //Debug.Log($"\n================================\n" +
        //    $"|GAME MANAGER - TestSave(string path)| \nSaving this data: \nPlayerPosition:{Player.transform.position} " +
        //    $"\nPlayer Golden Coins: {Player.GoldenCoins} \nPlayer Silver Coins: {Player.SilverCoins} \nPlayer Red Coins: {Player.RedCoins}" +
        //    $"\n================================\n");

        MSSTest.Save("playerPosition", Player.transform.position, path);
        MSSTest.Save("playerGoldenCoins", Player.GoldenCoins, path);
        MSSTest.Save("playerSilverCoins", Player.SilverCoins, path);
        MSSTest.Save("playerRedCoins", Player.RedCoins, path);
    }

    public void TestLoad(string path)
    {
        Player.gameObject.transform.position = MSSTest.Load("playerPosition", Vector3.zero, path);
        Player.GoldenCoins = MSSTest.Load("playerGoldenCoins", 1, path);
        Player.SilverCoins = MSSTest.Load("playerSilverCoins", 1, path);
        Player.RedCoins = MSSTest.Load("playerRedCoins", 1, path);
    }

    public static void SavePlayerData()
    {
        //Debug.Log($"\n================================\n" +
        //    $"|GAME MANAGER - SavePlayerData()| \nSaving this data: \nPlayerPosition:{Player.transform.position} " +
        //    $"\nPlayer Golden Coins: {Player.GoldenCoins} \nPlayer Silver Coins: {Player.SilverCoins} \nPlayer Red Coins: {Player.RedCoins}" +
        //    $"\n================================\n");

        MSS.Save("playerPosition", Player.transform.position, SaveFolderName, SaveFileName);
        MSS.Save("playerGoldenCoins", Player.GoldenCoins, SaveFolderName, SaveFileName);
        MSS.Save("playerSilverCoins", Player.SilverCoins, SaveFolderName, SaveFileName);
        MSS.Save("playerRedCoins", Player.RedCoins, SaveFolderName, SaveFileName);
    }

    public static void SavePlayerDataAtPath()
    {
        //Debug.Log($"\n================================\n" +
        //    $"|GAME MANAGER - SavePlayerData()| \nSaving this data: \nPlayerPosition:{Player.transform.position} " +
        //    $"\nPlayer Golden Coins: {Player.GoldenCoins} \nPlayer Silver Coins: {Player.SilverCoins} \nPlayer Red Coins: {Player.RedCoins}" +
        //    $"\n================================\n");

        MSSPath.Save("playerPosition", Player.transform.position, SaveFilePath);
        MSSPath.Save("playerGoldenCoins", Player.GoldenCoins, SaveFilePath);
        MSSPath.Save("playerSilverCoins", Player.SilverCoins, SaveFilePath);
        MSSPath.Save("playerRedCoins", Player.RedCoins, SaveFilePath);
    }

    public static void LoadPlayerData()
    {
        Player.transform.position = MSS.Load("playerPosition", SaveFilePath, Vector3.zero);
        Player.GoldenCoins = MSS.Load("playerGoldenCoins", SaveFilePath, 1);
        Player.SilverCoins = MSS.Load("playerSilverCoins", SaveFilePath, 1);
        Player.RedCoins = MSS.Load("playerRedCoins", SaveFilePath, 1);
    }

    public static void SetPath(string folderName, string fileName)
    {
        SaveFileName = fileName;
        SaveFolderName = folderName;
        SaveFilePath = MSSPath.CombinePersistent(SaveFolderName, SaveFileName);

        //Debug.Log($"\n================================\n" +
        //    $"|GAME MANAGER - SetPath()| \nSaveFile path was set at: {SaveFilePath}" +
        //    $"\n================================\n");
    }
    #endregion
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
