using UnityEngine;

// ResetPlayerSetting.cs
[System.Serializable]
public class ResetPlayerSetting : LevelSettingsOptionBase
{
    public override void Execute()
    {
        //GameManager.Instance.Load(); // Assuming Load resets health and relevant data
    }
}
