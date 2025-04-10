using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MultiSaveSystem
{
    public static class MSS
    {
        private static Dictionary<string, object> _data = new();

        #region Save directory and files values
        public static string SaveFolderName = "SaveFiles";
        public static string SaveFolderPath => Path.Combine(Application.persistentDataPath, SaveFolderName);

        public static string SaveFileName = "DefaultSaveFile";
        private static string SaveFilePath => SaveFolderPath + "/" + SaveFileName + ".mss";
        #endregion

        #region Saving variants
        public static void Save<T>(string key, T value, string path = null)
        {
            EnsureSaveDirectoryExists();
            _data[key] = value;

            string json = JsonUtility.ToJson(new Wrapper(_data), true);
            File.WriteAllText(path ?? SaveFilePath, json);
        }

        #endregion

        #region Loading Variants
        public static T Load<T>(string key, T defaultValue = default, string path = null)
        {
            if (!File.Exists(path ?? SaveFilePath)) return defaultValue;

            string json = File.ReadAllText(path ?? SaveFilePath);
            _data = JsonUtility.FromJson<Wrapper>(json).ToDictionary();

            if (_data.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            return defaultValue;
        }

        public static void LoadInto<T>(string key, ref T obj, string path = null)
        {
            if (!File.Exists(path ?? SaveFilePath))
            {
                Debug.LogError("File not found.");
                return;
            }

            string json = File.ReadAllText(path ?? SaveFilePath);
            _data = JsonUtility.FromJson<Wrapper>(json).ToDictionary();

            if (_data.TryGetValue(key, out var value) && value is T typedValue)
            {
                obj = typedValue;
            }
        }

        #endregion

        #region Save folder setting & safty checks
        public static void SetSaveFolder(string newDirectory)
        {
            SaveFolderName = newDirectory;
            EnsureSaveDirectoryExists();
        }
        
        public static void EnsureSaveDirectoryExists(string folderName = null)
        {
            if (folderName != null) SaveFolderName = folderName;
            if (!Directory.Exists(SaveFolderPath))
                Directory.CreateDirectory(SaveFolderPath);
        }

        public static void EnsureSaveFileExists(string saveName)
        {
            SaveFileName = saveName;
            EnsureSaveDirectoryExists();

            if (!Directory.Exists(SaveFilePath))
                Directory.CreateDirectory(SaveFilePath);
        }

        #endregion

        #region Get Path to Save File
        public static string GetSavePath(string saveName, string folderName=null)
        {
            EnsureSaveDirectoryExists(folderName);
            return Path.Combine(SaveFolderPath, saveName + ".mss");
        }
        #endregion

        #region Get All Save Files [2x Overloads]
        public static string[] GetAllSaveFiles(string folderName = null)
        {
            EnsureSaveDirectoryExists(folderName);
            return Directory.GetFiles(SaveFolderPath, "*.mss");
        }

        public static bool GetAllSaveFiles(out string[] saveFiles, string folderName = null)
        {
            EnsureSaveDirectoryExists(folderName);
            saveFiles = Directory.GetFiles(SaveFolderPath, "*.mss");
            if (saveFiles == null) return false;
            return true;
        }

        #endregion

        [Serializable]
        private class Wrapper
        {
            public List<string> keys = new();
            public List<string> jsonValues = new();
            public List<string> types = new();

            public Wrapper() { }

            public Wrapper(Dictionary<string, object> dict)
            {
                foreach (var kvp in dict)
                {
                    keys.Add(kvp.Key);
                    jsonValues.Add(JsonUtility.ToJson(kvp.Value));
                    types.Add(kvp.Value.GetType().AssemblyQualifiedName);
                }
            }

            public Dictionary<string, object> ToDictionary()
            {
                var dict = new Dictionary<string, object>();
                for (int i = 0; i < keys.Count; i++)
                {
                    var type = Type.GetType(types[i]);
                    if (type == null) continue;

                    var obj = JsonUtility.FromJson(jsonValues[i], type);
                    dict[keys[i]] = obj;
                }
                return dict;
            }
        }
    }
}
