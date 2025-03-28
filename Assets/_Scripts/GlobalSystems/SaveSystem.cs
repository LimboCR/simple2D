using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static SaveData s_saveData = new();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".simple";
        return saveFile;
    }

    public static void Save()
    {
       HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(s_saveData, true));
    }

    public static void HandleSaveData()
    {
        GameManager.Instance.Player.Save(ref s_saveData.PlayerData);
    }

    public static void Load()
    {
        string saveCurrent = File.ReadAllText(SaveFileName());

        s_saveData = JsonUtility.FromJson<SaveData>(saveCurrent);
        HandleLoadData();
    }

    public static void HandleLoadData()
    {
        GameManager.Instance.Player.Load(s_saveData.PlayerData);
    }
}
