using Limbo.CollectionUtils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static GlobalEventsManager;
using Limbo.CustomEditorAttributes;

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
    [ShowIf("PlayOnStart")] public string MusicToPlay = "DarkAmbient4";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        s_MainCameraSource = MainCameraSource;
        AddEventsListeners();

        AudioPacks = AudioPacksList.ToDictionaryInstanced(pack => Instantiate(pack),
            inst => inst.PackType, inst => inst.Initialize(), pack => pack != null);

        //if (AudioPacksList != null && AudioPacksList.Count > 0)
        //{
        //    foreach(var pack in AudioPacksList)
        //    {
        //        var packInstance = Instantiate(pack);
        //        packInstance.Initialize();
        //        AudioPacks.Add(packInstance.PackType, packInstance);
        //    }
        //}
    }
    void Start()
    {
        if(PlayOnStart) MainCameraSource.PlayTrack(AudioPacks[EAudioPackType.Soundtracks].TracksDictionary[MusicToPlay], AudioSourceType.Music);
    }

    
    void Update()
    {
        
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

    #endregion

    #region GameManager Sound Source Logic
    public static void GM_SFX_Play(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        if(playMode == PlayMode.safe)
            s_MainCameraSource.PlayTrack(clip, AudioSourceType.GameManager);
        else if(playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayTrack(clip, AudioSourceType.GameManager);
    }

    public static void GM_SFX_PlayRandom(PlayMode playMode, params AudioClip[] clips)
    {
        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayRandomTrack(AudioSourceType.GameManager, clips);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayRandomTrack(AudioSourceType.GameManager, clips);
    }

    public static void GM_SFX_ForceStop()
    {
        s_MainCameraSource.ForceStopPlaying(AudioSourceType.GameManager);
    }

    #endregion

    #region Music Sound Source Logic
    public static void MusicSourcePlay(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayTrack(clip, AudioSourceType.GameManager);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayTrack(clip, AudioSourceType.GameManager);
    }

    public static void MusicSourcePlayRandom(PlayMode playMode, params AudioClip[] clips)
    {
        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayRandomTrack(AudioSourceType.Music, clips);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayRandomTrack(AudioSourceType.Music, clips);
    }

    public static void MusicSourceForceStop()
    {
        s_MainCameraSource.ForceStopPlaying(AudioSourceType.Music);
    }

    #endregion

    #region Music Sound Source Logic
    public static void PlayerInterSourcePlay(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayTrack(clip, AudioSourceType.GameManager);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayTrack(clip, AudioSourceType.GameManager);
    }

    public static void PlayerInterSourcePlayRandom(PlayMode playMode, params AudioClip[] clips)
    {
        if (playMode == PlayMode.safe)
            s_MainCameraSource.PlayRandomTrack(AudioSourceType.PlayerConsumable, clips);
        else if (playMode == PlayMode.force)
            s_MainCameraSource.ForcePlayRandomTrack(AudioSourceType.PlayerConsumable, clips);
    }

    public static void PlayerInputSourceForceStop()
    {
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