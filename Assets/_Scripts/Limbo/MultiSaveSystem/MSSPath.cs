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
        public static string CombinePersistent(string directoryName, string fileName, bool isMSSFormat = true)
        {
            if(isMSSFormat) return Path.Combine(Application.persistentDataPath, directoryName, MSS.AutoFormat(fileName));
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
            path = FormatPath(path);
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
        public static T Load<T>(string key, string path, T defaultValue = default)
        {
            path = FormatPath(path);
            if (!File.Exists(path)) return defaultValue;

            string json = File.ReadAllText(path);
            _data = JsonUtility.FromJson<MSS.Wrapper>(json).ToDictionary();

            if (_data.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            return defaultValue;
        }

        #endregion

        /// <summary>
        /// Automatically formats the path to save file applying necessary format (<u><i>by default applies ".mss"</i></u>) if it's not formated in requested matter yet.
        /// </summary>
        /// <param name="path">Path to format</param>
        /// <param name="format">Format to apply (optional, default ".mss")</param>
        /// <returns></returns>
        public static string FormatPath(string path, string format = ".mss")
        {
            if (!path.EndsWith(format)) path += format;
            return path; ;
        }

        /// <summary>
        /// Builder class that helps create path and ensures it exists. Usefull for working with path-based saving and loading.
        /// </summary>
        public static class Builder
        {
            /// <summary>
            /// Name for the save folder to construct
            /// </summary>
            public static string SaveFolderName;
            /// <summary>
            /// Unsafe. Use only if <i><b>MSSPath.Builder.SaveFolderName (string)</b></i> is set.
            /// </summary>
            public static string SaveFolderPath => CombinePersistent(SaveFolderName);

            /// <summary>
            /// Ensures directory exists
            /// </summary>
            private static void EnsureDirectoryExists()
            {
                if (!Directory.Exists(SaveFolderPath))
                    Directory.CreateDirectory(SaveFolderPath);
            }

            /// <summary>
            /// Creates the required dirictory as part of path builder to ensure it exists
            /// </summary>
            /// <param name="folderName">Name of the directory</param>
            public static void CreatePath(string folderName)
            {
                SaveFolderName = folderName;
                EnsureDirectoryExists();
            }

            /// <summary>
            /// <b><u>|Safe|</u></b> Creates the directory if it does not exist and provides path to file inside it.
            /// </summary>
            /// <param name="fileName">File name</param>
            /// <param name="folderName">Directory name</param>
            /// <returns>Path to file inside the directory</returns>
            public static string GetSavePath(string fileName, string folderName)
            {
                SaveFolderName = folderName;
                EnsureDirectoryExists();
                return Path.Combine(SaveFolderPath, MSS.AutoFormat(fileName));
            }
        }
    }

}
///
///Path to the file with custom format inside folder