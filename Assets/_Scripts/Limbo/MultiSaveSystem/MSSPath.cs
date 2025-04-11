using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace MultiSaveSystem
{
    public class MSSPath
    {
        private static Dictionary<string, object> _data = new();

        #region Save directory and files values
        public static string SaveFolderName = "SaveFiles";
        public static string SaveFolderPath => Path.Combine(Application.persistentDataPath, SaveFolderName);

        public static string SaveFileName = "DefaultSaveFile";
        private static string SaveFilePath => SaveFolderPath + "/" + SaveFileName + ".mss";
        #endregion

        #region Combining path
        /// <summary>
        /// Creates correct path to saving folder using <i><u>Application.pressistentDataPath</u></i>
        /// </summary>
        /// <param name="directoryName">Name of the folder</param>
        /// <returns>Path to the folder</returns>
        public static string CombinePersistent(string directoryName)
        {
            return Path.Combine(Application.persistentDataPath, directoryName);
        }

        /// <summary>
        /// <br>Creates correct path to saving folder and file inside it using <i><u>Application.pressistentDataPath</u></i>.</br>
        /// <br>Optionally applies "<b><i><u>.mss</u></i></b>" format.</br>
        /// </summary>
        /// <param name="directoryName">Name of the folder</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="isMSSFormat"></param>
        /// <returns>Path to the file inside folder</returns>
        public static string CombinePersistent(string directoryName, string fileName, bool? isMSSFormat = null)
        {
            if((bool)isMSSFormat) return Path.Combine(Application.persistentDataPath, directoryName, fileName+".mss");
            return Path.Combine(Application.persistentDataPath, directoryName, fileName);
        }


        /// <summary>
        /// Creates correct path to saving folder and file inside it with custom formation using <i>Application.pressistentDataPath</i>
        /// </summary>
        /// <param name="directoryName">Name of the folder</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="fileFormat"></param>
        /// <returns>Path to the file with custom formating inside of the provided folder</returns>
        public static string CombinePersistent(string directoryName, string fileName, string fileFormat)
        {
            fileName += fileFormat;
            return Path.Combine(Application.persistentDataPath, directoryName, fileName + fileFormat);
        }
        #endregion

        #region Saving & Loading
        /// <summary>
        /// <br><b><u>|Safe|</u></b> If provided path does not exists - creates it</br>
        /// <br>Saves provided value at provided path. <u>Make sure to set file name at the end with format "<i>.mss</i>".</u></br>
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">Access key to value</param>
        /// <param name="value">Value to save</param>
        /// <param name="path">Path to save in</param>
        public static void Save<T>(string key, T value, string path)
        {
            if (!path.EndsWith(".mss")) path += ".mss";
            _data[key] = value;

            string json = JsonUtility.ToJson(new MSS.Wrapper(_data), true);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// <br><b><u>|Safe|</u></b> If there are no file in provided path - uses default value</br>
        /// <br>Loads value stored at provided path by key.</br>
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">Key to value</param>
        /// <param name="defaultValue">Safety value in case of null</param>
        /// <param name="path">Path to save file</param>
        /// <returns>Value that was requested by key. If null - defaultValue</returns>
        public static T Load<T>(string key, T defaultValue = default, string path = null)
        {
            if (!File.Exists(path)) return defaultValue;

            string json = File.ReadAllText(path);
            _data = JsonUtility.FromJson<MSS.Wrapper>(json).ToDictionary();

            if (_data.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            return defaultValue;
        }

        #endregion

        #region Path formats
        /// <summary>
        /// Ensures the path exsist. If not - creates it.
        /// </summary>
        /// <param name="path">Path to check</param>
        public static void EnsurePathExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        #endregion
    }

}
///
///Path to the file with custom format inside folder