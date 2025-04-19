using UnityEngine;

[System.Serializable]
public class LevelSettingEntry
{
    public ELevelSettingsType SettingsType = ELevelSettingsType.None;
    public LevelSettingsOptionBase Option;

    public void Apply()
    {
        switch (SettingsType)
        {
            case ELevelSettingsType.None: break;

            case ELevelSettingsType.CleanNPC:
                Option = new CleanNPCsSetting();
                Option.Execute();
                break;
            case ELevelSettingsType.ResetSpawnPoints:
                Option = new ResetSpawnPointsSetting();
                Option?.Execute();
                break;
            case ELevelSettingsType.ResetPlayer:
                Option = new ResetPlayerSetting();
                Option?.Execute();
                break;

            default: break;
        }

    }
}

public enum ELevelSettingsType
{
    None,
    CleanNPC,
    ResetSpawnPoints,
    ResetPlayer
}