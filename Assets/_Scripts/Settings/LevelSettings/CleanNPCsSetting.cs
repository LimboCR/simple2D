using UnityEngine;

[System.Serializable]
public class CleanNPCsSetting : LevelSettingsOptionBase
{
    public override void Execute()
    {
        GameManager.Instance.ClearCombatNPCS();
    }
}
