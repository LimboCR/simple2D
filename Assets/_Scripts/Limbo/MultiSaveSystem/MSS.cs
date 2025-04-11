using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace MultiSaveSystem
{
    public static class MSS
    {
        private static Dictionary<string, object> _data = new();

        #region Save directory and files values
        public static string SaveFolderName = "SaveFiles";
        public static string SaveFolderPath => Path.Combine(Application.persistentDataPath, SaveFolderName);

        public static string SaveFileName = "DefaultSaveFile";
        private static string SaveFilePath => Path.Combine(SaveFolderPath, "/" + SaveFileName + ".mss");

        #region Default
        public static string DefaulFolderName = "SaveFiles";
        public static string DefaultFolderPath => Path.Combine(Application.persistentDataPath, DefaulFolderName);
        #endregion

        #endregion

        #region Saving variants

        /// <summary>
        /// <br><b><u>|Safe|</u></b> If default folder does not exists - creates it</br>
        /// <br>Use only if using default folder for saving files. Saves file with default name in default folder</br>
        /// </summary>
        /// <typeparam name="T">Type of value to save</typeparam>
        /// <param name="key">Key to access saved value</param>
        /// <param name="value">Value to save</param>
        public static void Save<T>(string key, T value)
        {
            EnsureDirectoryExists();
            _data[key] = value;

            string json = JsonUtility.ToJson(new Wrapper(_data), true);
            File.WriteAllText(Path.Combine(DefaultFolderPath, "DefaultSaveFile.mss"), json);
        }

        /// <summary>
        /// <br><b><u>|Safe|</u></b> If provided folder does not exists - creates it</br>
        /// <br>Saves file with default name in provided folder.</br>
        /// </summary>
        /// <typeparam name="T">Type of value to save</typeparam>
        /// <param name="key">Key to access saved value</param>
        /// <param name="value">Value to save</param>
        /// <param name="folderName">Folder to save into</param>
        public static void Save<T>(string key, T value, string folderName)
        {
            EnsureDirectoryExists(folderName);
            _data[key] = value;

            string json = JsonUtility.ToJson(new Wrapper(_data), true);
            File.WriteAllText(SaveFilePath, json);
        }

        /// <summary>
        /// <br><b><u>|Safe|</u></b> If provided folder does not exists - creates it</br>
        /// <br>Saves file with provided name in provided folder.</br>
        /// </summary>
        /// <typeparam name="T">Type of value to save</typeparam>
        /// <param name="key">Key to access saved value</param>
        /// <param name="value">Value to save</param>
        /// <param name="folderName">Folder to save into</param>
        /// <param name="fileName">Name of the file (format applies automatiacelly as ".mss")</param>
        public static void Save<T>(string key, T value, string folderName, string fileName)
        {
            EnsureDirectoryExists(folderName);

            if (!fileName.EndsWith(".mss")) SaveFileName = fileName += ".mss";
            else SaveFileName = fileName;
            string path = MSSPath.CombinePersistent(folderName, SaveFileName);

            _data[key] = value;

            string json = JsonUtility.ToJson(new Wrapper(_data), true);
            File.WriteAllText(path, json);
        }

        #endregion

        #region Loading Variants
        public static T Load<T>(string key, string path, T defaultValue = default)
        {
            if (!path.EndsWith(".mss")) path += ".mss";

            if (!File.Exists(path)) return defaultValue;

            string json = File.ReadAllText(path);
            _data = JsonUtility.FromJson<Wrapper>(json).ToDictionary();

            if (_data.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }

            return defaultValue;
        }

        public static void LoadInto<T>(string key, ref T obj, string path)
        {
            if (!path.EndsWith(".mss")) path += ".mss";

            if (!File.Exists(path))
            {
                Debug.LogError($"File not found at {path}");
                return;
            }

            string json = File.ReadAllText(path);
            _data = JsonUtility.FromJson<Wrapper>(json).ToDictionary();

            if (_data.TryGetValue(key, out var value) && value is T typedValue)
            {
                obj = typedValue;
            }
        }

        #endregion

        #region Setting values
        public static void SetSaveFolder(string newDirectory)
        {
            SaveFolderName = newDirectory;
            EnsureDirectoryExists();
        }

        #endregion

        #region Directory Logics

        #region Safe Formats
        /// <summary>
        /// <br><u>Use only if using default folder for storing files.</u></br>
        /// <br>Ensures that default folder is created. If not - creates it.</br>
        /// </summary>
        public static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(DefaultFolderPath))
                Directory.CreateDirectory(DefaultFolderPath);
        }

        /// <summary>
        /// <b><u>|Safe|</u></b> Method that ensures that provided directory for saving files exists. If not - creates it.
        /// </summary>
        /// <param name="folderName">Name of the directory to check/create</param>
        public static void EnsureDirectoryExists(string folderName)
        {
            SaveFolderName = folderName;
            if (!Directory.Exists(SaveFolderPath))
                Directory.CreateDirectory(SaveFolderPath);
        }
        #endregion

        /// <summary>
        /// <b><u>|Safe|</u></b> Method that ensures that provided directory for save files exists. If not - creates it.
        /// </summary>
        /// <param name="folderName">Name of the directory to check/create</param>
        /// <param name="folderPath">Path to the directory for later usage</param>
        /// <param name="createDirectory">Create directory if it does not exist</param>
        /// <returns>Returns the path to directory</returns>
        public static bool EnsureDirectoryExists(string folderName, out string folderPath, bool createDirectory = true)
        {
            folderPath = MSSPath.CombinePersistent(folderName);
            if (!Directory.Exists(folderPath))
            {
                if (createDirectory) Directory.CreateDirectory(folderPath);
                else return false;
            }
            return true;
        }

        /// <summary>
        /// Use only if using default folder for storing files. Checks if default directory exists.
        /// </summary>
        /// <returns>True if directory exist, false if not</returns>
        public static bool DirectoryExist()
        {
            if (!Directory.Exists(DefaultFolderPath))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if provided directory exists.
        /// </summary>
        /// <param name="directoryName">Name of the directory</param>
        /// <returns>True if directory exist, false if not</returns>
        public static bool DirectoryExist(string directoryName)
        {
            if (!Directory.Exists(MSSPath.CombinePersistent(directoryName))) return false;
            return true;
        }

        /// <summary>
        /// Checks if provided directory existsm if yes - returns path to it.
        /// </summary>
        /// <param name="directoryName">Name of the directory</param>
        /// <param name="pathToDirectory">Path to the provided directory</param>
        /// <returns>Path to provided directory</returns>
        public static bool DirectoryExist(string directoryName, out string pathToDirectory)
        {
            pathToDirectory = MSSPath.CombinePersistent(directoryName);

            if (!Directory.Exists(pathToDirectory)) return false;
            return true;
        }

        /// <summary>
        /// <br>Checks if provided directory existsm if yes - returns path to it.</br>
        /// <br>If it's not created and createDirectory == false - returns nothing</br>
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <param name="createDirectory">If directory does not exist - create it.</param>
        /// <param name="pathToDirectory">Path to the provided directory.</param>
        /// <returns>Path to the provided directory.</returns>
        public static bool DirectoryExist(string directoryName, bool createDirectory, out string pathToDirectory)
        {
            pathToDirectory = MSSPath.CombinePersistent(directoryName);

            if (!Directory.Exists(pathToDirectory))
            {
                if(createDirectory) Directory.CreateDirectory(pathToDirectory);
                else return false;
            }
            return true;
        }

        #endregion

        #region Files Logics
        /// <summary>
        /// <br><u>Use only if using default folder for storing files.</u></br> 
        /// <br>Checks if save file with <i>".mss"</i> format exists in the default directory</br>
        /// </summary>
        /// <param name="fileName">File name (write without format)</param>
        /// <returns></returns>
        public static bool SaveExists(string fileName)
        {
            if (DirectoryExist())
            {
                string filePath = Path.Combine(DefaultFolderPath, fileName + ".mss");
                if (File.Exists(filePath)) return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// <br><u>Use only if using default folder for storing files.</u></br> 
        /// <br>Checks if save file with <i>".mss"</i> format exists in the default directory and returns path to it</br>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pathToFile"></param>
        /// <returns>Path to save file in default directory</returns>
        public static bool SaveExists(string fileName, out string pathToFile)
        {
            pathToFile = Path.Combine(DefaultFolderPath, AutoFormat(fileName));

            if (DirectoryExist())
            {
                if (File.Exists(pathToFile)) return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// Checks if save file with <i>".mss"</i> format exists in the provided directory
        /// </summary>
        /// <param name="fileName">File name (write without format)</param>
        /// <param name="folderName">Directory name</param>
        /// <returns>True if directory is correct and file exsits. False if dirrectory or file does not exist</returns>
        public static bool SaveExists(string fileName, string folderName)
        {
            if (DirectoryExist(folderName))
            {
                string filePath = MSSPath.CombinePersistent(folderName, fileName, true);
                if (File.Exists(filePath)) return true;
                else return false;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderName"></param>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public static bool SaveExists(string fileName, string folderName, out string pathToFile)
        {
            pathToFile = MSSPath.CombinePersistent(folderName, AutoFormat(fileName));
            if (DirectoryExist(folderName))
            {
                string filePath = MSSPath.CombinePersistent(folderName, fileName, true);
                if (File.Exists(filePath)) return true;
                else return false;
            }
            return false;
        }

        /// <summary>
        /// Use only if using default folder for storing files. Checks if save file exists in default directory and if not - creates it.
        /// </summary>
        /// <param name="saveName">Save file name</param>
        public static void EnsureSaveExists(string fileName)
        {
            SaveFileName = AutoFormat(fileName);
            EnsureDirectoryExists(DefaultFolderPath);

            if (!File.Exists(SaveFilePath))
                File.Create(SaveFilePath);
        }

        /// <summary>
        /// <br>Safe method to ensure if save file exists. If not - creates it.</br>
        /// <br>Also verifies provided directory and creates it if it does not exist</br>
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="directoryName">Name of the directory where it's located</param>
        public static void EnsureSaveExists(string fileName, string directoryName)
        {
            SaveFileName = AutoFormat(fileName);
            EnsureDirectoryExists(directoryName);

            if (!File.Exists(SaveFilePath))
                File.Create(SaveFilePath);
        }

        /// <summary>
        /// Cheks if save file exists in provided directory
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="saveFilePath"></param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public static bool EnsureSaveExists(string fileName, out string saveFilePath)
        {
            if (DirectoryExist())
            {
                saveFilePath = Path.Combine(DefaultFolderPath, AutoFormat(fileName));
                if (File.Exists(saveFilePath)) return true;
                else
                {
                    MSSDebug.FileError(fileName, saveFilePath);
                    saveFilePath = null;
                    return false;
                }
            }
            MSSDebug.DirectoryError(DefaulFolderName, DefaultFolderPath);
            saveFilePath = null;
            return false;
        }

        /// <summary>
        /// <br>Checks: <b>[1]</b> If provided directory exists. <b>[2]</b> If provided save file exists inside it.</br>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="directoryName"></param>
        /// <param name="saveFilePath"></param>
        /// <returns>Path to requested save file</returns>
        public static bool EnsureSaveExists(string fileName, string directoryName, out string saveFilePath)
        {
            if (EnsureDirectoryExists(directoryName, out string directoryPath, false))
            {
                saveFilePath = Path.Combine(directoryPath, AutoFormat(fileName));
                if (File.Exists(saveFilePath)) return true;
                else
                {
                    MSSDebug.FileError(fileName, saveFilePath);
                    saveFilePath = null;
                    return false;
                }
            }
            else
            {
                MSSDebug.DirectoryError(directoryName, MSSPath.CombinePersistent(directoryName));
                saveFilePath = null;
                return false;
            }
        }
        #endregion

        #region Get Path to Save File
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetSavePath(string fileName, string folderName=null)
        {
            EnsureDirectoryExists(folderName);
            return Path.Combine(SaveFolderPath, AutoFormat(fileName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderName"></param>
        /// <param name="saveFilePath"></param>
        /// <returns></returns>
        public static bool GetSavePath(string fileName, string folderName, out string saveFilePath)
        {
            saveFilePath = null;
            if (EnsureDirectoryExists(folderName, out string directoryPath, false))
            {
                if (SaveExists(fileName, folderName))
                {
                    saveFilePath = Path.Combine(directoryPath, AutoFormat(fileName));
                    return true;
                }
                else return false;
            }
            else return false;
        }
        #endregion

        #region Get All Save Files [2x Overloads]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string[] GetAllSaveFiles(string folderName)
        {
            EnsureDirectoryExists(folderName);
            return Directory.GetFiles(SaveFolderPath, "*.mss");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="saveFiles"></param>
        /// <returns></returns>
        public static bool GetAllSaveFiles(string folderName, out string[] saveFiles)
        {
            saveFiles = null;

            if (EnsureDirectoryExists(folderName, out string folderPath, false))
                saveFiles = Directory.GetFiles(folderPath, "*.mss");
            else return false;

            return true;
        }

        #endregion

        #region Get Latest/Oldest Save File

        #region Get Last Modification Time of the file
        /// <summary>
        /// Returns Last Modification Time for provided save file from default directory
        /// </summary>
        /// <param name="fileName">Save file name</param>
        /// <param name="editTime">Time when it was eddited</param>
        /// <returns>Time modification of last modified file</returns>
        public static bool GetLastEditTime(string fileName, out DateTime editTime)
        {
            editTime = DateTime.Now;

            if (EnsureSaveExists(fileName, DefaulFolderName, out string saveFilePath))
            {
                editTime = File.GetLastWriteTime(saveFilePath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns Last Modification Time for provided save file from provided folder
        /// </summary>
        /// <param name="fileName">Save file name</param>
        /// <param name="folderName">Folder name</param>
        /// <param name="editTime">Time of edit</param>
        /// <returns>Time modification of last modified file</returns>
        public static bool GetLastEditTime(string fileName, string folderName, out DateTime editTime)
        {
            editTime = DateTime.Now;
            if (EnsureSaveExists(fileName, folderName, out string saveFilePath))
            {
                editTime = File.GetLastWriteTime(saveFilePath);
                return true;
            }
            return false;
        }
        #endregion

        #region Get Latest Save File
        /// <summary>
        /// <br><u>Use only if want to go through default folder.</u></br> <br/>
        /// <br>Finds Latest Modified File and provides path to it</br>
        /// <br>Checks: <b>[1]</b> If there are any files in default folder. <b>[2]</b> If Defaul folder exists.</br>
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns>Path to last modified file</returns>
        public static bool GetLatestSave(out string pathToFile)
        {
            pathToFile = null;

            if (GetAllSaveFiles(DefaulFolderName, out string[] saveFiles))
            {
                List<DateTime> times = new();
                List<string> filePaths = new();
                foreach (string fileName in saveFiles)
                {
                    if (GetSavePath(fileName, DefaulFolderName, out string saveFilePath))
                    {
                        times.Add(File.GetLastWriteTime(saveFilePath));
                        filePaths.Add(saveFilePath);
                    }
                }

                DateTime latest = times.Max();
                int index = times.FindIndex(t => t == latest);

                pathToFile = filePaths[index];

                return true;
            }
            return false;
        }

        /// <summary>
        /// <br>Finds Latest Modified File and provides path to it</br>
        /// <br>Checks: <b>[1]</b> If there are any files in default folder. <b>[2]</b> If Defaul folder exists.</br>
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="pathToFile"></param>
        /// <returns>Path to last modified file</returns>
        public static bool GetLatestSave(string folderName, out string pathToFile)
        {
            pathToFile = null;

            if (GetAllSaveFiles(folderName, out string[] saveFiles))
            {
                List<DateTime> times = new();
                List<string> filePaths = new();
                foreach (string fileName in saveFiles)
                {
                    if (GetSavePath(fileName, folderName, out string saveFilePath))
                    {
                        times.Add(File.GetLastWriteTime(saveFilePath));
                        filePaths.Add(saveFilePath);
                    }
                }

                DateTime latest = times.Max();
                int index = times.FindIndex(t => t == latest);

                pathToFile = filePaths[index];

                return true;
            }
            return false;
        }
        #endregion

        #region Get Oldest Save File
        /// <summary>
        /// <br><u>Use only if want to go through default folder.</u></br> <br/>
        /// <br>Finds Oldest Modified file and provides path to it</br>
        /// <br>Checks: <b>[1]</b> If there are any files in default folder. <b>[2]</b> If Defaul folder exists.</br>
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns>Path to last modified file</returns>
        public static bool GetOldestSave(out string pathToFile)
        {
            pathToFile = null;

            if (GetAllSaveFiles(DefaulFolderName, out string[] saveFiles))
            {
                List<DateTime> times = new();
                List<string> filePaths = new();
                foreach (string fileName in saveFiles)
                {
                    if (GetSavePath(fileName, DefaulFolderName, out string saveFilePath))
                    {
                        times.Add(File.GetLastWriteTime(saveFilePath));
                        filePaths.Add(saveFilePath);
                    }
                }

                DateTime oldest = times.Min();
                int index = times.FindIndex(t => t == oldest);

                pathToFile = filePaths[index];

                return true;
            }
            return false;
        }

        /// <summary>
        /// <br>Finds Oldest Modified file and provides path to it</br>
        /// <br>Checks: <b>[1]</b> If there are any files in default folder. <b>[2]</b> If Defaul folder exists.</br>
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="pathToFile"></param>
        /// <returns>Path to last modified file</returns>
        public static bool GetOldestSave(string folderName, out string pathToFile)
        {
            pathToFile = null;

            if (GetAllSaveFiles(folderName, out string[] saveFiles))
            {
                List<DateTime> times = new();
                List<string> filePaths = new();
                foreach (string fileName in saveFiles)
                {
                    if (GetSavePath(fileName, folderName, out string saveFilePath))
                    {
                        times.Add(File.GetLastWriteTime(saveFilePath));
                        filePaths.Add(saveFilePath);
                    }
                }

                DateTime oldest = times.Min();
                int index = times.FindIndex(t => t == oldest);

                pathToFile = filePaths[index];

                return true;
            }
            return false;
        }
        #endregion

        #endregion

        #region Helper Functions
        /// <summary>
        /// <br>Checks if value to read/write is primitive or not</br>
        /// </summary>
        /// <param name="type">Type of the value</param>
        /// <returns>Type</returns>
        private static bool IsPrimitiveOrUnityType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Quaternion);
        }

        /// <summary>
        /// Automatically formats name of the save file if required
        /// </summary>
        /// <param name="fileName">Name of the file to format</param>
        /// <param name="format">Format to apply</param>
        /// <returns>Formatted File Name</returns>
        private static string AutoFormat(string fileName, string format = ".mss")
        {
            if (!fileName.EndsWith(format)) return fileName += format;
            else return fileName;
        }

        #endregion



        [Serializable]
        public class Wrapper
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

                    var value = kvp.Value;
                    if (IsPrimitiveOrUnityType(value.GetType()))
                    {
                        var wrapped = Activator.CreateInstance(typeof(PrimitiveWrapper<>).MakeGenericType(value.GetType()), value);
                        jsonValues.Add(JsonUtility.ToJson(wrapped));
                    }
                    else
                    {
                        jsonValues.Add(JsonUtility.ToJson(value));
                    }

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
                    
                    object obj;
                    if (IsPrimitiveOrUnityType(type))
                    {
                        var wrapperType = typeof(PrimitiveWrapper<>).MakeGenericType(type);
                        var wrapperObj = JsonUtility.FromJson(jsonValues[i], wrapperType);
                        obj = wrapperType.GetField("value").GetValue(wrapperObj);
                    }
                    else
                    {
                        obj = JsonUtility.FromJson(jsonValues[i], type);
                    }

                    dict[keys[i]] = obj;
                }
                return dict;
            }
        }

        [Serializable]
        public class PrimitiveWrapper<T>
        {
            public T value;

            public PrimitiveWrapper(T value)
            {
                this.value = value;
            }
        }

        [Serializable]
        private static class MSSDebug
        {
            public static void FileWarning(string fileName, string path = null)
            {
                if(path == null)
                {
                    Debug.LogWarning($"|MSS Warning| Requested save file does not exist!\nFile Name: {fileName}");
                    return;
                }
                else
                {
                    Debug.LogError($"|MSS Warning| Requested save file does not exist!" +
                            $"\nFile Name: {fileName}" +
                            $"\nPath to file: {path}");
                    return;
                }
            }

            public static void FileError(string fileName, string path = null)
            {
                if (path == null)
                {
                    Debug.LogError($"|MSS Error| Requested save file does not exist!\nFile Name: {fileName}");
                    return;
                }
                else
                {
                    Debug.LogError($"|MSS Error| Requested save file does not exist!" +
                            $"\nFile Name: {fileName}" +
                            $"\nPath to file: {path}");
                    return;
                }
            }

            public static void DirectoryWarning(string directoryName, string path = null)
            {
                if (path == null)
                {
                    Debug.LogError($"|MSS Warning| Requested directory does not exist!\nDirectory Name: {directoryName}");
                    return;
                }
                else
                {
                    Debug.LogError($"|MSS Warning| Requested directory does not exists!" +
                            $"\nDirectory Name: {directoryName}" +
                            $"\nDirectory Path: {path}");
                    return;
                }
            }

            public static void DirectoryError(string directoryName, string path = null)
            {
                if (path == null)
                {
                    Debug.LogError($"|MSS Error| Requested directory does not exist!\nDirectory Name: {directoryName}");
                    return;
                }
                else
                {
                    Debug.LogError($"|MSS Error| Requested directory does not exists!" +
                        $"\nDirectory name: {directoryName}" +
                        $"\nDirectory Path: {path}");
                    return;
                }
            }
        }

        public static class PathBuilder
        {
            public static string SaveFolderName = "SaveFiles";
            public static string SaveFolderPath => MSSPath.CombinePersistent(SaveFolderName);

            private static void EnsureDirectoryExists()
            {
                if (!Directory.Exists(SaveFolderPath))
                    Directory.CreateDirectory(SaveFolderPath);
            }

            public static void CreatePath(string folderName)
            {
                SaveFolderName = folderName;
                EnsureDirectoryExists();
            }


            public static string GetSavePath(string fileName)
            {
                EnsureDirectoryExists();
                return Path.Combine(SaveFolderPath, AutoFormat(fileName));
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
                return Path.Combine(SaveFolderPath, AutoFormat(fileName));
            }
        }

        public static class SaveSession
        {
            public static string ActiveSavePath = Application.persistentDataPath + "/save1.mss";

            public static void SetActiveSlot(int slotNumber)
            {
                ActiveSavePath = Application.persistentDataPath + $"/save{slotNumber}.save";
            }
        }
    }
}
