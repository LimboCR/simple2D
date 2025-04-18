using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MultiSaveSystem
{
    public static class MSSTest
    {
        private static Dictionary<string, object> _data = new();

        private static string DefaultPath => Application.persistentDataPath + "/default.mss";

        public static void Save<T>(string key, T value, string path = null)
        {
            if (!path.EndsWith(".mss")) path += ".mss";
            _data[key] = value;
            object val;
            _data.TryGetValue(key, out val);
            Debug.Log($"|Data| Key: {key}, Value: {val}");

            string json = JsonUtility.ToJson(new Wrapper(_data), true);
            File.WriteAllText(path ?? DefaultPath, json);
        }

        public static T Load<T>(string key, string path, T defaultValue = default)
        {
            if (!path.EndsWith(".mss")) path += ".mss";
            if (!File.Exists(path ?? DefaultPath)) return defaultValue;

            string json = File.ReadAllText(path ?? DefaultPath);
            _data = JsonUtility.FromJson<Wrapper>(json).ToDictionary();

            if (_data.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            return defaultValue;
        }

        public static class SaveSession
        {
            public static string ActiveSavePath = Application.persistentDataPath + "/save1.mss";

            public static void SetActiveSlot(int slotNumber)
            {
                ActiveSavePath = Application.persistentDataPath + $"/save{slotNumber}.mss";
            }
        }

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

