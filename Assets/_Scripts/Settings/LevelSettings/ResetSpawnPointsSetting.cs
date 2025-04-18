using UnityEngine;

// ResetSpawnPointsSetting.cs
[System.Serializable]
public class ResetSpawnPointsSetting : LevelSettingsOptionBase
{
    public override void Execute()
    {
        foreach (var spawner in GameManager.Instance.AllSpawnPointsAtScene)
        {
            //spawner.ResetSpawner();
        }
    }
}
