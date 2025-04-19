using UnityEngine;

// ResetSpawnPointsSetting.cs
[System.Serializable]
public class ResetSpawnPointsSetting : LevelSettingsOptionBase
{
    public override void Execute()
    {
        foreach (var spawn in GameManager.Instance.AllSpawnPointsAtScene)
        {
            if(spawn.TryGetComponent<SpawnPointHandler>(out SpawnPointHandler sph)){
                sph.ResetSpawnPoint();
            }
        }
    }
}
