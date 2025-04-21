using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalEventsManager;
using MultiSaveSystem;
using Limbo.CollectionUtils;
using Unity.Cinemachine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    ////     Variables     \\\\
    #region Instance
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
    #endregion

    #region Game States
    [Header("GameStates")]
    [SerializeField] public static GameStates currentGameState;
    [SerializeField] public GameStates GameStateDisplay;

    #endregion

    #region Scripts and objects references
    public GameObject PlayerVisualObject;
    private static NewPlayerController _player;
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

    //public GameObject PlayerPrefab;
    //public GameObject PlayerReference;
    //public CinemachineCamera CinemachineCamera;
    //public static GameObject s_PlayerPrefab;

    #endregion

    #region NPC References at scene
    [Header("All NPCs & spawn point references")]
    public List<GameObject> AllEnemiesAtScene = new();
    public List<GameObject> AllFriendlyAtScene = new();
    public List<GameObject> AllPeacfullAtScene = new();
    public List<GameObject> AllSpawnPointsAtScene = new();
    public List<GameObject> AllTriggerZones = new();

    [Header("Sectorial reference")]
    public List<GameObject> EnemiesAtSector = new();
    public List<GameObject> FriendlyAtSector = new();
    public List<GameObject> PeacfullAtSector = new();
    public List<GameObject> SpawnPointsAtSector = new();

    #endregion

    #region Global LayerMasks settings
    [Space]
    [Header("==== GLOBAL LAYERMASKS ====")]

    [Space]
    [Header("Navigation & Waypoints Remove/Add")]
    public LayerMask LayersToRemoveFromNavigation, LayersToAddToNavigation;

    [Header("Props")]
    public LayerMask LayersToRemoveFromProps, LayersToAddToProps;
    [Header("Items")]
    public LayerMask LayersToRemoveFromItems, LayersToAddToItems;

    [Header("Destructable")]
    public LayerMask LayersToRemoveFromDestructable, LayersToAddToDestructable;
    [Header("Interactable")]
    public LayerMask LayersToRemoveFromInteractable, LayersToAddToInteractable;

    [Header("Combat Enemy Remove/Add")]
    public LayerMask LayersToRemoveFromCombatEnemies, LayersToAddToCombatEnemies;
    [Header("Combat Friendlies Remove/Add")]
    public LayerMask LayersToRemoveFromCombatFriendlies, LayersToAddToCombatFriendlies;
    [Header("Peacfull")]
    public LayerMask LayersToRemoveFromPeacfull, LayerToAddToPeacfull;

    [Header("Player")]
    public LayerMask LayersToRemoveFromPlayer, LayerToAddToPlayer;
    #region Static Variables

    #endregion

    #endregion

    #region Editor Variables
    [HideInInspector] public bool foldout1, foldout2, foldout3, foldout4, foldout5, layerMsksFoldout1, layerMsksFoldout2, layerMsksFoldout3, layerMsksFoldout4, layerMsksFoldout5, layerMsksFoldout6, layerMsksFoldout7, layerMsksFoldout8;
    #endregion

    #region Saving Variables
    //Saving data
    [Space]
    [Header("Save things")]
    public static string SaveFileName;
    public static string SaveFolderName;
    public static string SaveFilePath;

    public static bool LoadingSaveFile;
    #endregion

    #region Inspector
    [HideInInspector] public string SaveFileNameDispaly = "SaveFileName", SaveFolderDispaly = "SaveFolderName", SaveFilePathDisplay = "/path/to/saveFile.mss";
    #endregion

    #region SFX
    //Sounds
    [SerializeField] private List<SoundContainer> SoundsList;
    public Dictionary<string, AudioClip> GetGMSounds => Sounds;
    private static Dictionary<string, AudioClip> Sounds;
    #endregion

    #region Scene Specific Variables
    [Space, Header("Current Scene Data")]
    public static GameObject InitialPlayerPosition;
    public static string CurrentSceneName;
    public static int CurrentSceneIndex;
    public static string PastScene;

    [Space, Header("Cross Scene Data")]
    public PersistentPlayerData CrossSceneDataContainer;
    public static PersistentPlayerData s_CrossSceneDataContainer;
    public static bool PlayerLeavingScene;
    public static bool PlayerArrivedToScene;
    public static bool SpawnPlayerAtInitial;
    #endregion
    bool LoadSaveRef;
    //=======================\\

    ////     Logic     \\\\
    #region Awake, Start, Update, Fixed Update and etc
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        GlobalSettingsManager.Instance.GMHasAccess = true;

        InitialPlayerPosition = GameObject.Find("PlayerInitial_Position");
        s_CrossSceneDataContainer = CrossSceneDataContainer;

        SceneManager.sceneLoaded += OnSceneLoaded;

        EventSubscriber();
        Sounds = SoundsList.ToDictionarySafe(sound => sound.Key, sound => sound.Sound);
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Player == null)
            Debug.Log($"[Update] GameManagerInstance {this.GetInstanceID()} still has null Player at runtime.");

        GameStateDisplay = currentGameState;
        SaveFileNameDispaly = SaveFileName;
        SaveFolderDispaly = SaveFolderName;
        SaveFilePathDisplay = SaveFilePath;

        GoThroughStates();

        // For testing saving & loading
        if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
        {
            PerformQuickSave();
        }
        if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
        {
            GlobalSettingsManager.Command = EGMCommandType.LoadSave;
            PerformQuickLoad();
        }
        
    }

    private void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSaveRef = GlobalSettingsManager.LoadFromSaveFile;
        Debug.Log($"LoadSaveRef: {LoadSaveRef}");
        PlayerArrivedToScene = true;

        var currentScene = SceneManager.GetActiveScene();
        CurrentSceneName = currentScene.name;
        CurrentSceneIndex = currentScene.buildIndex;

        s_CrossSceneDataContainer = CrossSceneDataContainer;

        InitialPlayerPosition = GameObject.Find("PlayerInitial_Position");
        if (InitialPlayerPosition == null)
            Debug.LogWarning($"[GameManager {this.GetInstanceID()}] [OnSceneLoaded] No initial position for the player at this scene");

        _player = null;

        StartCoroutine(AwaitPlayer());
    }

    public IEnumerator AwaitPlayer(float timeout = 10f)
    {
        float timer = 0f;
        while (Player == null && timer < timeout)
        {
            Player = FindAnyObjectByType<NewPlayerController>();
            timer += Time.deltaTime;
            yield return null;
        }

        if (Player == null)
        {
            Debug.LogWarning("[AwaitPlayer] Player not found in time.");
            yield break;
        }

        PlayerVisualObject = Player.gameObject;

        if(GlobalSettingsManager.Command == EGMCommandType.LoadSave)
        {
            TryLoad();

            PlayerLeavingScene = false;
            PlayerArrivedToScene = false;
            SpawnPlayerAtInitial = false;

            GlobalSettingsManager.Command = EGMCommandType.None;

            yield break;
        }
        else if (GlobalSettingsManager.Command == EGMCommandType.NewGame)
        {
            SpawnPlayerAtInitial = true;
        }

        //Debug.LogWarning($"[AwaitPlayer] Before our bug value LoadFromSaveFile = {GlobalSettingsManager.LoadFromSaveFile}");
        if (SpawnPlayerAtInitial)
        {
            if (InitialPlayerPosition != null)
            {
                Debug.LogWarning("Setting player position to his initial position.");
                Player.transform.position = InitialPlayerPosition.transform.position;
            }
            else
            {
                Debug.LogWarning("InitialPlayerPosition is null, cannot set player position.");
            }
        }
        else
        {
            Debug.Log("[AwaitPlayer] Skipping InitialPlayerPosition override due to LoadFromSaveFile");
        }

        // Always do this part regardless
        if (PlayerLeavingScene && PlayerArrivedToScene)
        {
            if (CrossSceneDataContainer != null) MigratePlayerData();
            else Debug.LogError("[AwaitPlayer] CrossSceneDataContainer Is Null");
        }

        Player.ResetPlayer();

        PlayerLeavingScene = false;
        PlayerArrivedToScene = false;
        SpawnPlayerAtInitial = false;

        yield break;
    }

    internal static void MigratePlayerData()
    {
        s_CrossSceneDataContainer.LoadToPlayer(Player);
        s_CrossSceneDataContainer.LoadToTimeManager(FindAnyObjectByType<TimeManager>());

        SendCoinsChanged(ECollectable.Golden, Player.GoldenCoins);
        SendCoinsChanged(ECollectable.Silver, Player.SilverCoins);
        SendCoinsChanged(ECollectable.Red, Player.RedCoins);
    }

    internal static void PlayerIntializedLeavingScene()
    {
        PlayerLeavingScene = true;
        SpawnPlayerAtInitial = true;
        if (s_CrossSceneDataContainer != null)
        {
            s_CrossSceneDataContainer.SaveFromPlayer(Player);
            s_CrossSceneDataContainer.SaveFromTimeManager(FindAnyObjectByType<TimeManager>());
        }
    }

    internal static void SetPlayerPosAtInitial()
    {
        SpawnPlayerAtInitial = true;
    }
    #endregion

    #region General
    public GameObject GetPlayerGameObject()
    {
        if (Player != null) return Player.gameObject;
        return null;
    }
    private void EventSubscriber()
    {
        OnStateChange.AddListener(ChangeGameState);
        OnNPCSpawn.AddListener(AddNPCAtScene);
        OnNPCRemove.AddListener(RemoveNPCAtScene);
        OnSpawnAdd.AddListener(AddSpawnPointAtScene);
        OnSpawnRemove.AddListener(RemoveSpawnPointAtScene);
    }
    private void ChangeGameState(GameStates state)
    {
        currentGameState = state;
    }
    #endregion

    #region Event Based Fucntions

    #region Scene tracker

    #region NPCs At Scene
    private void AddNPCAtScene(GameObject npc, NPCType type)
    {
        if (AllEnemiesAtScene.Contains(npc) || AllFriendlyAtScene.Contains(npc) || AllPeacfullAtScene.Contains(npc))
        {
            Debug.LogWarning($"NPC: {npc.name} of type {type} is already added to one of the All NPCs at scene lists");
            return;
        }

        switch (type)
        {
            case NPCType.CloseCombat:
                if (npc.TryGetComponent<IMultiCharacterData>(out IMultiCharacterData data))
                {
                    if (data.CharacterTeam == 0) AllFriendlyAtScene.Add(npc);
                    else if (data.CharacterTeam == 1) AllEnemiesAtScene.Add(npc);
                    else Debug.LogWarning("NPCTeam was unknown");

                    if (AllEnemiesAtScene.Contains(npc) && AllFriendlyAtScene.Contains(npc))
                        Debug.LogError($"NPC: {npc.name} Can't be in both AllFriendlyNPCAtScene and AllEnemiesAtScene Lists at the same time.");
                    else if (AllPeacfullAtScene.Contains(npc))
                        Debug.LogError($"NPC: {npc.name} Can't be in AllPeacfullAtScene List and combat NPCs List at the same time.");
                }
                break;

            case NPCType.Peacful:
                AllPeacfullAtScene.Add(npc);
                break;

            case NPCType.Trader:
                break;

            default: break;
        }
    }
    private void RemoveNPCAtScene(GameObject npc, NPCType type)
    {
        //if (!AllEnemiesAtScene.Contains(npc) || !AllFriendlyAtScene.Contains(npc) || !AllPeacfullAtScene.Contains(npc))
        //{
        //    Debug.LogWarning($"NPC: {npc.name} of type {type} isn't in any lists of the All NPCs at scene ");
        //    return;
        //}

        switch (type)
        {
            case NPCType.CloseCombat:
                if(npc.TryGetComponent<IMultiCharacterData>(out IMultiCharacterData data))
                {
                    if (data.CharacterTeam == 0)
                    {
                        if (AllFriendlyAtScene.Contains(npc)) AllFriendlyAtScene.Remove(npc);
                        else Debug.LogWarning($"NPC: {npc.name} Wasn't in AllFriendlyNPCAtScene List");
                    }
                    else if (data.CharacterTeam == 1)
                    {
                        if (AllEnemiesAtScene.Contains(npc)) AllEnemiesAtScene.Remove(npc);
                        else Debug.LogWarning($"NPC: {npc.name} Wasn't in AllEnemiesAtScene List");
                    }
                    else Debug.LogWarning("NPCTeam was unknown");
                }
                break;

            case NPCType.Peacful:
                if (AllPeacfullAtScene.Contains(npc)) AllPeacfullAtScene.Remove(npc);
                else Debug.LogWarning($"NPC: {npc.name} wasnt in AllPeacfullAtScene List");
                break;

            case NPCType.Trader:
                break;

            default: break;
        }
    }
    #endregion

    #region Spawn Points At Scene
    private void AddSpawnPointAtScene(GameObject spawnPoint)
    {
        AllSpawnPointsAtScene.Add(spawnPoint);
    }
    private void RemoveSpawnPointAtScene(GameObject spawnPoint)
    {
        AllSpawnPointsAtScene.Remove(spawnPoint);
    }
    #endregion

    #endregion

    #region GameStates
    private static void GoThroughStates()
    {
        switch (currentGameState)
        {
            case GameStates.QuickSave:
                PerformQuickSave();
                break;
            case GameStates.Load:
                PerformQuickLoad();
                break;

            default: break;
        }
    }

    #endregion

    #endregion

    #region Save System Callers

    public static void PerformQuickSave()
    {
        currentGameState = GameStates.QuickSave;
        BroadcastActualGameState(currentGameState);

        SaveFileName = "QuickSave";
        SaveFolderName = "QuickSaves";
        SavePlayerData();

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    public static void PerformQuickLoad()
    {

        currentGameState = GameStates.Restart;
        BroadcastActualGameState(currentGameState);
        
        GlobalSettingsManager.LoadFromSaveFile = true;

        SaveFileName = "QuickSave";
        SaveFolderName = "QuickSaves";
        SaveFilePath = MSSPath.CombinePersistent(SaveFolderName, SaveFileName);

        if (MSS.SaveExists(SaveFileName, SaveFolderName, out string pathToFile))
        {
            LoadPlayerData(pathToFile);
        }
        else GlobalSettingsManager.LoadFromSaveFile = false;

        currentGameState = GameStates.Play;
        BroadcastActualGameState(currentGameState);
    }

    public static void TryLoad()
    {
        ClearCombatNPCS();
        ResetSpawnPoints();
        GlobalEventsManager.ResetTriggerZones();

        SaveFileName = "QuickSave";
        SaveFolderName = "QuickSaves";
        if (MSS.SaveExists(SaveFileName, SaveFolderName, out string pathToFile))
        {
            GlobalSettingsManager.LoadFromSaveFile = true;

            string saveFileScene = null;
            MSS.LoadInto("sceneName", ref saveFileScene, pathToFile);

            if(saveFileScene != null)
            {
                if(CurrentSceneName != saveFileScene)
                {
                    PastScene = CurrentSceneName;
                    SceneManager.LoadScene(saveFileScene);
                }
            }
            LoadPlayerData(pathToFile);
        }
        else
        {
            GlobalSettingsManager.LoadFromSaveFile = false;
            Debug.Log("File Does not exists reseting to the beginning");
            if (InitialPlayerPosition != null)
            {
                Debug.Log("Initial player position is found. Setting the position");
                Player.transform.position = InitialPlayerPosition.transform.position;
                Player.ResetPlayer();
            }
        }
    }

    //////////////// Unused \\\\\\\\\\\\\\\\\\\\\
    public static void PerformSave(string saveFileName)
    {
        //currentGameState = GameStates.QuickSave;
        //BroadcastActualGameState(currentGameState);

        //currentGameState = GameStates.Play;
        //BroadcastActualGameState(currentGameState);
    }
    public static void PerformLoading()
    {
        //currentGameState = GameStates.Load;
        //BroadcastActualGameState(currentGameState);

        //currentGameState = GameStates.Play;
        //BroadcastActualGameState(currentGameState);
    }
    #endregion

    #region Save System Functions
    //public void TestSave(string path)
    //{
    //    MSSTest.Save("playerPosition", Player.transform.position, path);
    //    MSSTest.Save("playerGoldenCoins", Player.GoldenCoins, path);
    //    MSSTest.Save("playerSilverCoins", Player.SilverCoins, path);
    //    MSSTest.Save("playerRedCoins", Player.RedCoins, path);
    //}

    //public void TestLoad(string path)
    //{
    //    Player.gameObject.transform.position = MSSTest.Load("playerPosition", path, Vector3.zero);
    //    Player.GoldenCoins = MSSTest.Load("playerGoldenCoins", path, 1);
    //    Player.SilverCoins = MSSTest.Load("playerSilverCoins", path, 1);
    //    Player.RedCoins = MSSTest.Load("playerRedCoins", path, 1);
    //}

    public static void SavePlayerData()
    {
        ShowNotification("Saving game..");
        #region Player Common
        if(CurrentSceneName != null)
        {
            MSS.Save("sceneName", CurrentSceneName, SaveFolderName, SaveFileName);
        }
        
        MSS.Save("playerPosition", Player.transform.position, SaveFolderName, SaveFileName);
        #endregion

        #region Player Stats
        MSS.Save("playerHealth", Player.CurrentHealth, SaveFolderName, SaveFileName);
        MSS.Save("playerMaxHealth", Player.MaxHealth, SaveFolderName, SaveFileName);
        #endregion

        #region Player Currency & Levels
        MSS.Save("playerGoldenCoins", Player.GoldenCoins, SaveFolderName, SaveFileName);
        MSS.Save("playerSilverCoins", Player.SilverCoins, SaveFolderName, SaveFileName);
        MSS.Save("playerRedCoins", Player.RedCoins, SaveFolderName, SaveFileName);
        MSS.Save("playerSkillPoints", Player.SkillPoints, SaveFolderName, SaveFileName);
        MSS.Save("playerLevel", Player.PlayerLevel, SaveFolderName, SaveFileName);

        #endregion

        Player.CurrentHealth = Player.MaxHealth;
        ResetHealthBar(Player.CurrentHealth);

        ShowNotification("Game Saved");
        if (Sounds.TryGetValue("Save", out AudioClip clip)) PlayGMSfx(clip);
    }

    public static void LoadPlayerData()
    {
        ShowNotification("Loading game..");
        GlobalSettingsManager.LoadFromSaveFile = true;

        #region Scene to load
        string saveFileScene = null;
        MSS.LoadInto("sceneName", ref saveFileScene, SaveFilePath);

        if (saveFileScene != null)
        {
            if (CurrentSceneName != saveFileScene)
            {
                PastScene = CurrentSceneName;
                SceneManager.LoadScene(saveFileScene);
            }
        }
        #endregion

        #region Player Common
        Player.transform.position = MSS.Load("playerPosition", SaveFilePath, Vector3.zero);
        #endregion

        #region Player Stats
        Player.MaxHealth = MSS.Load("playerMaxHealth", SaveFilePath, 100);
        #endregion

        #region Player Currency & Levels
        Player.GoldenCoins = MSS.Load("playerGoldenCoins", SaveFilePath, 0);
        SendCoinsChanged(ECollectable.Golden, Player.GoldenCoins);

        Player.SilverCoins = MSS.Load("playerSilverCoins", SaveFilePath, 0);
        SendCoinsChanged(ECollectable.Silver, Player.SilverCoins);

        Player.RedCoins = MSS.Load("playerRedCoins", SaveFilePath, 0);
        SendCoinsChanged(ECollectable.Red, Player.RedCoins);

        Player.SkillPoints = MSS.Load("playerSkillPoints", SaveFilePath, 0);
        Player.PlayerLevel = MSS.Load("playerLevel", SaveFilePath, 0);
        #endregion

        Player.ResetPlayer();
        GlobalSettingsManager.LoadFromSaveFile = false;

        ShowNotification("Loading complete");
        if (Sounds.TryGetValue("Load", out AudioClip clip)) PlayGMSfx(clip);
    }

    public static void LoadPlayerData(string pathToFile)
    {
        ShowNotification("Loading game..");
        GlobalSettingsManager.LoadFromSaveFile = true;

        #region Scene to load
        string saveFileScene = null;
        MSS.LoadInto("sceneName", ref saveFileScene, pathToFile);

        if (saveFileScene != null)
        {
            if (CurrentSceneName != saveFileScene)
            {
                SceneManager.LoadScene(saveFileScene);
            }
        }
        #endregion

        #region Player Common
        Player.transform.position = MSS.Load("playerPosition", pathToFile, Vector3.zero);
        #endregion

        #region Player Stats
        Player.MaxHealth = MSS.Load("playerMaxHealth", pathToFile, 100);
        #endregion

        #region Player Currency & Levels
        Player.GoldenCoins = MSS.Load("playerGoldenCoins", pathToFile, 0);
        SendCoinsChanged(ECollectable.Golden, Player.GoldenCoins);

        Player.SilverCoins = MSS.Load("playerSilverCoins", pathToFile, 0);
        SendCoinsChanged(ECollectable.Silver, Player.SilverCoins);

        Player.RedCoins = MSS.Load("playerRedCoins", pathToFile, 0);
        SendCoinsChanged(ECollectable.Red, Player.RedCoins);

        Player.SkillPoints = MSS.Load("playerSkillPoints", pathToFile, 0);
        Player.PlayerLevel = MSS.Load("playerLevel", pathToFile, 0);
        #endregion

        Player.ResetPlayer();
        GlobalSettingsManager.LoadFromSaveFile = false;

        ShowNotification("Loading complete");
        if (Sounds.TryGetValue("Load", out AudioClip clip)) PlayGMSfx(clip);
    }

    public static void SetPath(string folderName, string fileName)
    {
        SaveFileName = fileName;
        SaveFolderName = folderName;
        SaveFilePath = MSSPath.CombinePersistent(SaveFolderName, SaveFileName);
    }
    #endregion

    #region Reset Functions
    public static void ClearCombatNPCS()
    {
        foreach(var npc in instance.AllEnemiesAtScene)
        {
            if(npc.TryGetComponent<CloseCombatNPCBase>(out CloseCombatNPCBase npcBase))
            {
                npcBase.DestroyOnRequest();
            }
        }
        instance.AllEnemiesAtScene.Clear();

        foreach (var npc in instance.AllFriendlyAtScene)
        {
            if (npc.TryGetComponent<CloseCombatNPCBase>(out CloseCombatNPCBase npcBase))
            {
                npcBase.DestroyOnRequest();
            }
        }
        instance.AllFriendlyAtScene.Clear();
    }

    public static void ResetSpawnPoints()
    {
        foreach(var spawn in instance.AllSpawnPointsAtScene)
        {
            if(spawn.TryGetComponent<SpawnPointHandler>(out SpawnPointHandler sph))
            {
                sph.ResetSpawnPoint();
            }
        }
    }

    public static void ResetTriggerZones()
    {
        foreach(var trigger in instance.AllTriggerZones)
        {
            trigger.GetComponent<TriggerZone>().IsArmed = true;
            trigger.SetActive(true);
        }
    }
    #endregion

    //=======================\\
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
