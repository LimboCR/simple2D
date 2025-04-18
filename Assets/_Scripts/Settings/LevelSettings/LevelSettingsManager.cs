// LevelSettingsManager.cs
using System.Collections.Generic;
using UnityEngine;

public class LevelSettingsManager : MonoBehaviour
{
    [SerializeField] private List<LevelSettingEntry> settingsToApply;

    public void ApplySettings()
    {
        foreach (var setting in settingsToApply)
        {
            setting.Apply();
        }
    }
}