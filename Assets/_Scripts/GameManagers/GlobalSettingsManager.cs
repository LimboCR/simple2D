using UnityEngine;
using MultiSaveSystem;
using UnityEngine.SceneManagement;

public class GlobalSettingsManager : MonoBehaviour
{
    private static GlobalSettingsManager instance;
    public static GlobalSettingsManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return null;

            if (instance == null)
                Instantiate(Resources.Load<GlobalSettingsManager>("GlobalSettingsManager"));
#endif
            return instance;
        }
    }

    public ButtonBindingsSO PlayerButtonBindings;
    public static ButtonBindingsSO s_PlayerButtonBindings;

    public AudioSettingsSO GlobalAudioSettings;
    public static AudioSettingsSO s_GlobalAudioSettings;

    public GraphicsSettingsSO GraphicsSettings;
    public static GraphicsSettingsSO s_GraphicsSettings;

    //public GameManagerCommands GameManagerCommands;
    //public static GameManagerCommands s_GameManagerCommands;

    [Header("Check if GM has access")]
    public bool GMHasAccess = false;

    [Space, Header("GameManager Commands")]
    public static EGMCommandType Command;
    public static bool LoadFromSaveFile;
    public static bool SpawnPlayerAtInitial;
    public static bool MovementBetweenScenes;

    public bool UseGameManagerCommandsData = false;

    [Space, Header("Save files details")]
    public static string SaveFileName = "QuickSave";
    public static string SaveFolderName = "QuickSaves";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        s_PlayerButtonBindings = PlayerButtonBindings;
        s_GlobalAudioSettings = GlobalAudioSettings;
        s_GraphicsSettings = GraphicsSettings;
        //s_GameManagerCommands = GameManagerCommands;
    }

    public static bool CheckForSaveFile()
    {
        if (MSS.SaveExists("QuickSave", "QuickSaves")) return true;
        return false;
    }

    //public static void LoadLastSave()
    //{
    //    Instance.UseGameManagerCommandsData = true;

        
    //}


    #region Main Menu
    public static void StartNewGame()
    {
        SpawnPlayerAtInitial = true;
        Command = EGMCommandType.NewGame;
        SceneManager.LoadScene("Level_0 Training");
    }

    public static void LoadSaveFile()
    {
        if (MSS.SaveExists("QuickSave", "QuickSaves", out string pathToFile))
        {
            Command = EGMCommandType.LoadSave;

            string sceneRef = MSS.Load("sceneName", pathToFile, "");
            if (sceneRef != null && sceneRef != "") SceneManager.LoadScene(sceneRef);
        }
        else Debug.LogWarning("No save file exists yet");
    }
    #endregion
}

public enum EGMCommandType
{
    None,
    NewGame,
    LoadSave
}