using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "PersistentAudioSettings" ,menuName = "Game/Audio Settings")]
public class AudioSettingsSO : ScriptableObject
{
    public float MasterVolumeDB;
    public float MusicVolumeDB;
    public float CombatVolumeDB;
    public float EnviromentVolumeDB;
    public float NatureVolumeDB;
    public float NotificationVolumeDB;

    public void SaveVolumeData(GlobalSettingsManager settingsManager)
    {
        MasterVolumeDB = settingsManager.GetMixerVal("MasterVolume");
        MusicVolumeDB = settingsManager.GetMixerVal("MusicVolume");
        CombatVolumeDB = settingsManager.GetMixerVal("CombatVolume");
        EnviromentVolumeDB = settingsManager.GetMixerVal("EnviromentVolume");
        NatureVolumeDB = settingsManager.GetMixerVal("NatureVolume");
        NotificationVolumeDB = settingsManager.GetMixerVal("NotificationVolume");
    }

    public void LoadVolumeData(GlobalSettingsManager settingsManager)
    {
        settingsManager.SetMixerVal("MasterVolume", MasterVolumeDB);
        settingsManager.SetMixerVal("MusicVolume", MusicVolumeDB);
        settingsManager.SetMixerVal("CombatVolume", CombatVolumeDB);
        settingsManager.SetMixerVal("EnviromentVolume", EnviromentVolumeDB);
        settingsManager.SetMixerVal("NatureVolume", NatureVolumeDB);
        settingsManager.SetMixerVal("NotificationVolume", NotificationVolumeDB);
    }
}
