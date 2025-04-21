using Limbo.CollectionUtils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static GlobalEventsManager;
using Limbo.CustomEditorAttributes;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return null;

            if (instance == null)
                Instantiate(Resources.Load<AudioManager>("AudioManager"));
#endif
            return instance;
        }
    }


    [Space, Header("Audio Mixer")]
    public AudioMixer Mixer;
    public static AudioMixer s_Mixer;

    [Space]
    [Header("Mandatory Sound Sorces")]
    public AudioSourcesController MainCameraSource;
    public static AudioSourcesController s_MainCameraSource;

    [Space]
    [Header("Audio Clips Packs")]
    public List<AudioClipsPacksSO> AudioPacksList;
    public Dictionary<EAudioPackType, AudioClipsPacksSO> AudioPacks;

    [Space, Header("Play Music On Start")]
    public bool PlayOnStart = false;
    public bool Loop = false;
    [ShowIf("PlayOnStart")] public string MusicToPlay = "DarkAmbient4";

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

        if (MainCameraSource == null)
        {
            MainCameraSource = FindAnyObjectByType<AudioSourcesController>();
        }
        s_MainCameraSource = MainCameraSource;

        AddEventsListeners();

        AudioPacks = AudioPacksList.ToDictionaryInstanced(pack => Instantiate(pack),
            inst => inst.PackType, inst => inst.Initialize(), pack => pack != null);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        s_MainCameraSource = null;

        if (MainCameraSource == null)
            StartCoroutine(AwaitForAudioSources());
    }

    private IEnumerator AwaitForAudioSources()
    {
        while (MainCameraSource == null)
        {
            MainCameraSource = FindAnyObjectByType<AudioSourcesController>();
        }

        s_MainCameraSource = MainCameraSource;
        yield break;
    }

    void Start()
    {
        if(PlayOnStart) MainCameraSource.PlayTrack(AudioPacks[EAudioPackType.Soundtracks].TracksDictionary[MusicToPlay], AudioSourceType.Music, Loop);
    }

    
    void Update()
    {
        
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

    #region Add Listeners
    private void AddEventsListeners()
    {
        OnGameManagerSFXPlay.AddListener(GM_SFX_Play);
        OnGameManagerSFXPlayRandom.AddListener(GM_SFX_PlayRandom);

        OnMusicSourcePlay.AddListener(MusicSourcePlay);
        OnMusicSourcePlayRandom.AddListener(MusicSourcePlayRandom);

        OnPlayerInteractionPlay.AddListener(PlayerInterSourcePlay);
        OnPlayerInteractionPlayRandom.AddListener(PlayerInterSourcePlayRandom);

        OnForceStopPlaying.AddListener(IntakeStopPlaying);
    }

    #endregion

    #region Common functions
    /// <summary>
    /// Stops Playing all sources
    /// </summary>
    public static void IntakeStopPlaying()
    {
        MusicSourceForceStop();
        GM_SFX_ForceStop();
        PlayerInputSourceForceStop();
    }

    /// <summary>
    /// Stops playing source of specific type
    /// </summary>
    /// <param name="type">Type of the source</param>
    public static void IntakeStopPlaying(AudioSourceType type)
    {
        switch (type)
        {
            case AudioSourceType.Music:
                MusicSourceForceStop();
                break;

            case AudioSourceType.GameManager:
                GM_SFX_ForceStop();
                break;

            case AudioSourceType.PlayerConsumable:
                PlayerInputSourceForceStop();
                break;

            default: break;
        }
    }

    public static bool IsSourcePlaying(AudioSourceType type)
    {
        if (s_MainCameraSource == null)
        {
            Debug.LogError("MainCameraSource is null");
            return false;
        }

        return s_MainCameraSource.IsPlaying(type);
    }

    #endregion

    #region GameManager Sound Source Logic
    public static void GM_SFX_Play(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        if (s_MainCameraSource == null) return;

        if(playMode == PlayMode.safe)
            s_MainCameraSource.PlayTrack(clip, AudioSourceType.GameManager, false);
        else if(playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayTrack(clip, AudioSourceType.GameManager, false);
    }

    public static void GM_SFX_PlayRandom(PlayMode playMode, params AudioClip[] clips)
    {
        if (s_MainCameraSource == null) return;

        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayRandomTrack(AudioSourceType.GameManager, false, clips);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayRandomTrack(AudioSourceType.GameManager, false, clips);
    }

    public static void GM_SFX_ForceStop()
    {
        if (s_MainCameraSource == null) return;

        s_MainCameraSource.ForceStopPlaying(AudioSourceType.GameManager);
    }

    #endregion

    #region Music Sound Source Logic
    public static void MusicSourcePlay(AudioClip clip, PlayMode playMode = PlayMode.safe, bool loop = false)
    {
        if (s_MainCameraSource == null) return;

        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayTrack(clip, AudioSourceType.GameManager, loop);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayTrack(clip, AudioSourceType.GameManager, loop);
    }

    public static void MusicSourcePlayRandom(PlayMode playMode, bool loop = false, params AudioClip[] clips)
    {
        if (s_MainCameraSource == null) return;

        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayRandomTrack(AudioSourceType.Music, loop, clips);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayRandomTrack(AudioSourceType.Music, loop, clips);
    }

    public static void MusicSourceForceStop()
    {
        if (s_MainCameraSource == null) return;

        s_MainCameraSource.ForceStopPlaying(AudioSourceType.Music);
    }

    #endregion

    #region Music Sound Source Logic
    public static void PlayerInterSourcePlay(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        if (s_MainCameraSource == null) return;

        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayTrack(clip, AudioSourceType.GameManager, false);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayTrack(clip, AudioSourceType.GameManager, false);
    }

    public static void PlayerInterSourcePlayRandom(PlayMode playMode, params AudioClip[] clips)
    {
        if (s_MainCameraSource == null) return;

        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayRandomTrack(AudioSourceType.PlayerConsumable, false, clips);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayRandomTrack(AudioSourceType.PlayerConsumable, false, clips);
    }

    public static void PlayerInputSourceForceStop()
    {
        if (s_MainCameraSource == null) return;

        s_MainCameraSource.ForceStopPlaying(AudioSourceType.PlayerConsumable);
    }

    #endregion

    public static AudioMixer GetMixer()
    {
        return Instance.Mixer;
    }
}

public enum PlayMode
{
    force,
    safe
}