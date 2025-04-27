using UnityEngine;
using MultiSaveSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

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
    [Space, Header("Settings Menu")]
    public GameObject SettingsMenu;
    public GameObject GraphicSettingsTab;
    public GameObject AudioSettingsTab;
    public static GameObject s_SettingsMenu;

    #region Volume Variables
    [Space, Header("Volume Settings")]
    #region Sliders
    public Slider MasterVolumeSlider;
    public static Slider s_MasterVolumeSlider;

    public Slider MusicVolumeSlider;
    public static Slider s_MusicVolumeSlider;

    public Slider CombatVolumeSlider;
    public static Slider s_CombatVolumeSlider;

    public Slider EnviromentVolumeSlider;
    public static Slider s_EnviromentVolumeSlider;

    public Slider NatureVolumeSlider;
    public static Slider s_NatureVolumeSlider;

    public Slider NotificationVolumeSlider;
    public static Slider s_NotificationVolumeSlider;
    #endregion

    [Header("Text Fields")]
    #region Text Fields
    public TMP_Text MasterVolumeText;
    public static TMP_Text s_MasterVolumeText;

    public TMP_Text MusicVolumeText;
    public static TMP_Text s_MusicVolumeText;

    public TMP_Text EnviromentVolumeText;
    public static TMP_Text s_EnviromentVolumeText;

    public TMP_Text NatureVolumeText;
    public static TMP_Text s_NatureVolumeText;

    public TMP_Text NotificationVolumeText;
    public static TMP_Text s_NotificationVolumeText;

    public TMP_Text CombatVolumeText;
    public static TMP_Text s_CombatVolumeText;
    #endregion

    [Space, Header("Audio Settings")]
    public AudioMixer GlobalMixer;
    public static AudioMixer s_GlobalMixer;

    #endregion

    [Space, Header("GameManager Commands")]
    public static EGMCommandType Command;
    public static bool LoadFromSaveFile;
    public static bool SpawnPlayerAtInitial;
    public static bool MovementBetweenScenes;

    public bool UseGameManagerCommandsData = false;

    [Space, Header("Save files details")]
    public static string SaveFileName = "QuickSave";
    public static string SaveFolderName = "QuickSaves";


    private GameObject LoadButton;
    private bool InSettings = false;

    #region Awake, Start and staff
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        StaticAppliers();

        StartCoroutine(CoroutineUtils.WaitUntil(() => s_GlobalAudioSettings != null,() => s_GlobalAudioSettings.LoadVolumeData(Instance)));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InSettings)
            {
                InSettings = !InSettings;
                ShowSettingsMenu();
            }
        }
    }

    private void StaticAppliers()
    {
        s_GlobalMixer = GlobalMixer;
        s_GlobalAudioSettings = GlobalAudioSettings;

        s_SettingsMenu = SettingsMenu;
        
        s_PlayerButtonBindings = PlayerButtonBindings;
        s_GraphicsSettings = GraphicsSettings;
        s_MasterVolumeSlider = MasterVolumeSlider;
        s_MusicVolumeSlider = MusicVolumeSlider;
        s_CombatVolumeSlider = CombatVolumeSlider;
        s_EnviromentVolumeSlider = EnviromentVolumeSlider;
        s_NatureVolumeSlider = NatureVolumeSlider;
        s_NotificationVolumeSlider = NotificationVolumeSlider;

        s_MasterVolumeText = MasterVolumeText;
        s_MusicVolumeText = MusicVolumeText;
        s_CombatVolumeText = CombatVolumeText;
        s_EnviromentVolumeText = EnviromentVolumeText;
        s_NatureVolumeText = NatureVolumeText;
        s_NotificationVolumeText = NotificationVolumeText;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            LoadButton = GameObject.Find("Load Game");
            if (LoadButton != null) AbleToLoad();
        }
    }

    public static bool CheckForSaveFile()
    {
        if (MSS.SaveExists("QuickSave", "QuickSaves")) return true;
        return false;
    }

    private void AbleToLoad()
    {
        if (CheckForSaveFile())
        {
            Debug.Log("Able to load the game");
            LoadButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    #endregion

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

    #region Settings General
    public static void ShowSettingsMenu()
    {
        if (s_SettingsMenu.activeSelf)
        {
            s_SettingsMenu.SetActive(false);
            Instance.InSettings = false;
        }
        else if (!s_SettingsMenu.activeSelf)
        {
            ShowGraphicsTab();
            s_SettingsMenu.SetActive(true);
            Instance.InSettings = true;
        }
    }

    public static void ShowGraphicsTab()
    {
        Instance.AudioSettingsTab.SetActive(false);
        Instance.GraphicSettingsTab.SetActive(true);
    }

    public static void ShowAudioTab()
    {
        GetMasterVolume();
        Instance.GraphicSettingsTab.SetActive(false);
        Instance.AudioSettingsTab.SetActive(true);
    }
    #endregion

    #region AudioSettings
    // group[0] - Master Volume | [1] = Music | [2] = combat | [3] = Enviroment | [4] = Nature | [5] = UiAndNotifications
    public static void GetMasterVolume()
    {
        s_GlobalAudioSettings.LoadVolumeData(Instance);

        AudioMixerGroup[] group = s_GlobalMixer.FindMatchingGroups("Master");

        if (s_GlobalMixer.GetFloat("MasterVolume", out float dBVolume))
        {
            s_MasterVolumeText.text = DBToPercent(dBVolume) + "%";
            s_MasterVolumeSlider.value = DecibelToLinear(dBVolume);
        }

        if (s_GlobalMixer.GetFloat("MusicVolume", out float dBVolume1))
        {
            s_MusicVolumeText.text = DBToPercent(dBVolume1) + "%";
            s_MusicVolumeSlider.value = DecibelToLinear(dBVolume1);
        }

        if (s_GlobalMixer.GetFloat("CombatVolume", out float dBVolume2))
        {
            s_CombatVolumeText.text = DBToPercent(dBVolume2) + "%";
            s_CombatVolumeSlider.value = DecibelToLinear(dBVolume2);
        }

        if (s_GlobalMixer.GetFloat("EnviromentVolume", out float dBVolume3))
        {
            s_EnviromentVolumeText.text = DBToPercent(dBVolume3) + "%";
            s_EnviromentVolumeSlider.value = DecibelToLinear(dBVolume3);
        }

        if (s_GlobalMixer.GetFloat("NatureVolume", out float dBVolume4))
        {
            s_NatureVolumeText.text = DBToPercent(dBVolume4) + "%";
            s_NatureVolumeSlider.value = DecibelToLinear(dBVolume4);
        }

        if (s_GlobalMixer.GetFloat("NotificationVolume", out float dBVolume5))
        {
            s_NotificationVolumeText.text = DBToPercent(dBVolume5) + "%";
            s_NotificationVolumeSlider.value = DecibelToLinear(dBVolume5);
        }
    }

    public void OnMasterVolumeSliderChanged()
    {
        UpdateValueFromSlider(s_MasterVolumeSlider.value, EVolumeSettingsType.Master);
    }

    public void OnMusicVolumeSliderChanged()
    {
        UpdateValueFromSlider(s_MusicVolumeSlider.value, EVolumeSettingsType.Music);
    }

    public void OnCombatVolumeSliderChanged()
    {
        UpdateValueFromSlider(s_CombatVolumeSlider.value, EVolumeSettingsType.Combat);
    }

    public void OnEnviromentVolumeSliderChanged()
    {
        UpdateValueFromSlider(s_EnviromentVolumeSlider.value, EVolumeSettingsType.Enviroment);
    }

    public void OnNatureVolumeSliderChanged()
    {
        UpdateValueFromSlider(s_NatureVolumeSlider.value, EVolumeSettingsType.Nature);
    }

    public void OnNotificationVolumeSliderChanged()
    {
        UpdateValueFromSlider(s_NotificationVolumeSlider.value, EVolumeSettingsType.Notifiications);
    }

    private void UpdateValueFromSlider(float sliderValue, EVolumeSettingsType settingsType)
    {
        float dB = LinearToDecibel(sliderValue);

        if (settingsType == EVolumeSettingsType.Master)
        {
            s_GlobalMixer.SetFloat("MasterVolume", dB);
            SetVolumeTextByDB(s_MasterVolumeText, dB);
        }

        if (settingsType == EVolumeSettingsType.Music)
        {
            s_GlobalMixer.SetFloat("MusicVolume", dB);
            SetVolumeTextByDB(s_MusicVolumeText, dB);
        }

        if (settingsType == EVolumeSettingsType.Combat)
        {
            s_GlobalMixer.SetFloat("CombatVolume", dB);
            SetVolumeTextByDB(s_CombatVolumeText, dB);
        }

        if (settingsType == EVolumeSettingsType.Enviroment)
        {
            s_GlobalMixer.SetFloat("EnviromentVolume", dB);
            SetVolumeTextByDB(s_EnviromentVolumeText, dB);
        }

        if (settingsType == EVolumeSettingsType.Nature)
        {
            s_GlobalMixer.SetFloat("NatureVolume", dB);
            SetVolumeTextByDB(s_NatureVolumeText, dB);
        }

        if (settingsType == EVolumeSettingsType.Notifiications)
        {
            s_GlobalMixer.SetFloat("NotificationVolume", dB);
            SetVolumeTextByDB(s_NotificationVolumeText, dB);
        }

        GlobalAudioSettings.SaveVolumeData(this);
    }

    public AudioMixer GetAudioMixer()
    {
        return GlobalMixer;
    }

    public float GetMixerVal(string valueName)
    {
        if(s_GlobalMixer.GetFloat(valueName, out float dBVolume)) return dBVolume;

        else return 0f;
    }

    public void SetMixerVal(string valueName, float dBValue)
    {
        s_GlobalMixer.SetFloat(valueName, dBValue);
    }

    #region Set Text Values
    private static void SetVolumeTextByDB(TMP_Text textToSet, float dBVolume)
    {
        textToSet.text = DBToPercent(dBVolume) + "%";
    }

    private static void SetVolumeTextByLinear(TMP_Text textToSet, float linear)
    {
        textToSet.text = LinearToPercent(linear) + "%";
    }

    private static void SetVolumeTextByPercent(TMP_Text textToSet, float percent)
    {
        textToSet.text = percent + "%";
    }
    #endregion

    #region Convert To Percent
    static float DBToPercent(float dBVolume)
    {
        float linear = DecibelToLinear(dBVolume); // value between 0.0 and 1.0
        return Mathf.Round(linear * 100f);
    }

    static float LinearToPercent(float linear)
    {
        return Mathf.Round(linear * 100f);
    }

    #endregion


    #region Convert Linear <> DB
    // Converts 0.0–1.0 to decibels (logarithmic)
    static float LinearToDecibel(float linear)
    {
        return Mathf.Approximately(linear, 0f) ? -80f : Mathf.Log10(linear) * 20f;
    }

    // Converts decibels to 0.0–1.0 slider value
    static float DecibelToLinear(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }
    #endregion

    #endregion
}

public enum EGMCommandType
{
    None,
    NewGame,
    LoadSave
}

public enum EVolumeSettingsType
{
    Master,
    Music,
    Combat,
    Nature,
    Enviroment,
    Notifiications
}