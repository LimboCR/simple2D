using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalEventsManager;
using MultiSaveSystem;
using Limbo.CollectionUtils;
using Unity.Cinemachine;
using System;

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

    [Space, Header("Cross Scene Data")]
    public PersistentPlayerData CrossSceneDataContainer;
    public static PersistentPlayerData s_CrossSceneDataContainer;
    public static bool PlayerLeavingScene;
    public static bool PlayerArrivedToScene;
    #endregion

    //=======================\\

    ////     Logic     \\\\
    #region Awake, Start, Update, Fixed Update and etc
    private void Awake()
    {
        InitialPlayerPosition = GameObject.Find("PlayerInitial_Position");
        s_CrossSceneDataContainer = CrossSceneDataContainer;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        EventSubscriber();
        Sounds = SoundsList.ToDictionarySafe(sound => sound.Key, sound => sound.Sound);

        Player = FindAnyObjectByType<NewPlayerController>();
    }

    private void Start()
    {
        if (Player != null && !Player.AllSet) Player.ResetPlayer();
    }

    private void Update()
    {
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
            PerformQuickLoad();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (InitialPlayerPosition != null) InitialPlayerPosition = null;

        InitialPlayerPosition = GameObject.Find("PlayerInitial_Position");
        if (InitialPlayerPosition == null) Debug.LogWarning("No initial position for the player at this scene");

        Player.transform.position = InitialPlayerPosition.transform.position;
        if (PlayerLeavingScene == true && PlayerArrivedToScene == true)
        {
            if(CrossSceneDataContainer != null)
            {
                CrossSceneDataContainer.LoadToPlayer(Player);
                Player.ResetPlayer();
            }
        }
    }

    internal static void PlayerIntializedLeavingScene()
    {
        PlayerLeavingScene = true;
        if(s_CrossSceneDataContainer != null) s_CrossSceneDataContainer.SaveFromPlayer(Player);
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
        if (!AllEnemiesAtScene.Contains(npc) || !AllFriendlyAtScene.Contains(npc) || !AllPeacfullAtScene.Contains(npc))
        {
            Debug.LogWarning($"NPC: {npc.name} of type {type} isn't in any lists of the All NPCs at scene ");
            return;
        }

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

        SaveFileName = "QuickSave";
        SaveFolderName = "QuickSaves";
        SaveFilePath = MSSPath.CombinePersistent(SaveFolderName, SaveFileName);
        LoadPlayerData();

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

    public static void TryLoad()
    {
        

        Debug.Log("Trying to load");
        SaveFileName = "QuickSave";
        SaveFolderName = "QuickSaves";
        if (MSS.SaveExists(SaveFileName, SaveFolderName, out string pathToFile)){
            Debug.Log("File exists");
            LoadPlayerData(pathToFile);
        }
        else
        {
            Debug.Log("File Does not existsm reseting to the beginning");
            if (InitialPlayerPosition != null)
            {
                Debug.Log("Initial player position is found. Using it as leverage");
                Player.transform.position = InitialPlayerPosition.transform.position;
                Player.ResetPlayer();
            }
        }
    }

    #endregion

    #region Save System Functions
    public void TestSave(string path)
    {
        MSSTest.Save("playerPosition", Player.transform.position, path);
        MSSTest.Save("playerGoldenCoins", Player.GoldenCoins, path);
        MSSTest.Save("playerSilverCoins", Player.SilverCoins, path);
        MSSTest.Save("playerRedCoins", Player.RedCoins, path);
    }

    public void TestLoad(string path)
    {
        Player.gameObject.transform.position = MSSTest.Load("playerPosition", path, Vector3.zero);
        Player.GoldenCoins = MSSTest.Load("playerGoldenCoins", path, 1);
        Player.SilverCoins = MSSTest.Load("playerSilverCoins", path, 1);
        Player.RedCoins = MSSTest.Load("playerRedCoins", path, 1);
    }

    public static void SavePlayerData()
    {
        ShowNotification("Saving game..");
        #region Player Common
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

        ShowNotification("Game Saved");
        if (Sounds.TryGetValue("Save", out AudioClip clip)) PlayGMSfx(clip);
    }

    public static void LoadPlayerData()
    {
        ShowNotification("Loading game..");
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

        ShowNotification("Loading complete");
        if (Sounds.TryGetValue("Load", out AudioClip clip)) PlayGMSfx(clip);
    }

    public static void LoadPlayerData(string pathToFile)
    {
        ShowNotification("Loading game..");
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
    public void ClearCombatNPCS()
    {
        foreach(var npc in AllEnemiesAtScene)
        {
            if(npc.TryGetComponent<CloseCombatNPCBase>(out CloseCombatNPCBase npcBase))
            {
                npcBase.DestroyOnRequest();
            }
        }
        AllEnemiesAtScene.Clear();

        foreach (var npc in AllFriendlyAtScene)
        {
            if (npc.TryGetComponent<CloseCombatNPCBase>(out CloseCombatNPCBase npcBase))
            {
                npcBase.DestroyOnRequest();
            }
        }
        AllFriendlyAtScene.Clear();
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
