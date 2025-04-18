using UnityEngine;

[System.Serializable]
public class LevelSettingEntry
{
    public ELevelSettingsType SettingsType = ELevelSettingsType.None;
    public LevelSettingsOptionBase Option;

    public void Apply()
    {
        Option?.Execute();
    }
}

public enum ELevelSettingsType
{
    None,
    CleanNPCs,
    ResetSpawnPoints,
    ResetPlayer
}