using GeoChatter.Extensions;
using GeoChatter.Helpers;
using GeoChatter.Core.Model;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Core.Storage
{
    /// <summary>
    /// Initial implementation. Needs much more work. ;)
    /// </summary>
    public class DataStorage
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DataStorage));
        private static DataStorage storage;
        /// <summary>
        /// Persistant storage object
        /// </summary>
        /// <returns></returns>
        public static DataStorage GetDefaultStorage(string path = "", string extension = "")
        {
            if (storage == null)
            {
                storage = new DataStorage(path, extension, true);
            }

            return storage;
        }

        private readonly bool useDailyStorage;

        private string defaultStoragePath = "records";
        private string defaultExtension = ".json";

        private string DayStoragePath { get; set; }

        private string GetStoragePath(bool dailyStorage)
        {
            return dailyStorage ? DayStoragePath : defaultStoragePath;
        }

        /// <summary>
        /// 
        /// </summary>
        public DataStorage(string path = "", string extension = "", bool createDailyStorageFolders = false)
        {
            useDailyStorage = createDailyStorageFolders;
            InitializeStorage(path, extension);
        }
        /// <summary>
        /// Initializes the DataStorage
        /// </summary>
        /// <param name="path">Custom path, overwrites appSettings</param>
        /// <param name="extension"></param>
        public void InitializeStorage(string path = "", string extension = "")
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                defaultStoragePath = path;
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                defaultExtension = extension;
            }

            try
            {
                if (!Directory.Exists(defaultStoragePath))
                {
                    Directory.CreateDirectory(defaultStoragePath);
                }
                if (useDailyStorage)
                {
                    DayStoragePath = Path.Combine(defaultStoragePath, DateTime.Now.ToGeoChatterTimestamp());
                    if (!Directory.Exists(DayStoragePath))
                    {
                        Directory.CreateDirectory(DayStoragePath);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        /// <summary>
        /// Saves an object to a json file named after the type
        /// </summary>
        /// <typeparam name="T">Type of object to save</typeparam>
        /// <param name="objToSave">The object to save</param>
        public void Save<T>(T objToSave)
        {
            string filename = typeof(T).Name;
            try
            {
                if (typeof(T) == typeof(TableOptions))
                {
                    filename = "TableOptions";
                }
                else if (typeof(T) == typeof(Dictionary<string, string>))
                {
                    filename = "strings";
                    using StreamWriter sw2 = File.CreateText(Path.ChangeExtension(filename, defaultExtension));
                    sw2.Write(JsonConvert.SerializeObject(objToSave));
                    return;
                }

                using StreamWriter sw = File.CreateText(Path.Combine(defaultStoragePath, Path.ChangeExtension(filename, defaultExtension)));
                sw.Write(JsonConvert.SerializeObject(objToSave));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        /// <summary>
        /// Loads an object from the corresponding JSON file
        /// </summary>
        /// <typeparam name="T">Type of object to load</typeparam>
        /// <returns>The loaded object</returns>
        public T Load<T>()
        {
            string filename = Path.Combine(defaultStoragePath, Path.ChangeExtension(typeof(T).ToString(), defaultExtension));

            object players = null;
            try
            {
                if (File.Exists(filename))
                {
                    players = JsonConvert.DeserializeObject(File.ReadAllText(filename), typeof(T));
                }
                return (T)players;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
            return default;
        }

        /// <summary>
        /// Reads the given file
        /// </summary>
        /// <param name="filename">The filename with extension included</param>
        /// <returns></returns>
        public string ReadOtherFile(string filename)
        {
            string fileToOpen = Path.Combine(defaultStoragePath, filename);
            try
            {
                if (File.Exists(fileToOpen))
                {
                    return File.ReadAllText(fileToOpen);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }

            return string.Empty;
        }

        /// <summary>
        /// Reads a given file from the root or daily folder
        /// </summary>
        /// <param name="filename">The filename of the file to open</param>
        /// <param name="dailyStorage">Whether the file is located inside DailyStorage</param>
        /// <returns></returns>
        public Tuple<string, string> ReadFile(string filename, bool dailyStorage = false)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(filename, nameof(filename));
            try
            {
                filename = Path.ChangeExtension(filename, defaultExtension);
                string fileToOpen = Path.Combine(GetStoragePath(dailyStorage), filename);
                if (!File.Exists(fileToOpen))
                {
                    fileToOpen = searchDailyFolders(filename);
                }

                if (File.Exists(fileToOpen))
                {
                    return new Tuple<string, string>(fileToOpen, File.ReadAllText(fileToOpen));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }

            return new Tuple<string, string>(string.Empty, string.Empty);
        }

        /// <summary>
        /// Delete file with name <paramref name="filename"/>
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dailyStorage"></param>
        /// <returns></returns>
        public bool DeleteFile(string filename, bool dailyStorage = false)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(filename, nameof(filename));

            try
            {
                filename = Path.ChangeExtension(filename, defaultExtension);
                string fileToOpen = Path.Combine(GetStoragePath(dailyStorage), filename);
                if (!File.Exists(fileToOpen))
                {
                    fileToOpen = searchDailyFolders(filename);
                }

                if (File.Exists(fileToOpen))
                {
                    File.Delete(fileToOpen);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Searches through the daily folders for a file
        /// </summary>
        /// <param name="filename">The name of the file to search for</param>
        /// <returns></returns>
        private string searchDailyFolders(string filename)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(filename, nameof(filename));

            try
            {
                DirectoryInfo directory = new(defaultStoragePath);
                foreach (DirectoryInfo dir in directory.GetDirectories())
                {
                    foreach (FileInfo file in dir.GetFiles("*" + defaultExtension))
                    {
                        if (file.Name == filename)
                        {
                            return file.FullName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }

            return filename;
        }

        /// <summary>
        /// Reads all files in the root or daily folder
        /// </summary>
        /// <param name="dailyStorage"></param>
        /// <param name="subfolders"></param>
        /// <returns></returns>
        public Dictionary<string, string> ReadAllFiles(bool dailyStorage = false, bool subfolders = false)
        {
            string path = GetStoragePath(dailyStorage);
            return ReadAllFiles(path, subfolders);

        }
        /// <summary>
        /// Reads all files in the given folder
        /// </summary>
        /// <param name="path">The path to load, relative to the executable path</param>
        /// <param name="subfolders"></param>
        /// <returns></returns>
        public Dictionary<string, string> ReadAllFiles(string path, bool subfolders = false)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(path, nameof(path));
            Dictionary<string, string> result = new();
            if (subfolders)
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    LoadFiles(dir, result);
                }
            }
            else
            {
                LoadFiles(path, result);
            }

            return result;
        }

        private void LoadFiles(string path, Dictionary<string, string> result)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(path, nameof(path));
            foreach (string fileName in Directory.GetFiles(path, $"*{defaultExtension}"))
            {
                try
                {
                    string json = File.ReadAllText(fileName);
                    string fname = new FileInfo(fileName).Name.ReplaceDefault(new FileInfo(fileName).Extension, string.Empty);
                    if (result.ContainsKey(fname))
                    {
                        result[fname] = json;
                    }
                    else
                    {
                        result.Add(fname, json);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Summarize());
                }
            }
        }
        /// <summary>
        /// Write a file named <paramref name="filename"/> with <paramref name="content"/> using extension <see cref="defaultExtension"/>
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        /// <param name="dailyStorage"></param>
        /// <returns></returns>
        public string WriteFile(string filename, string content, bool dailyStorage = false)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(filename, nameof(filename));
            string finalfilename = Path.Combine(GetStoragePath(dailyStorage), Path.ChangeExtension(filename, defaultExtension));
            try
            {
                File.WriteAllText(finalfilename, content);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
            return finalfilename;
        }
        /// <summary>
        /// Write a file named <paramref name="filename"/> with <paramref name="content"/> using extension from <paramref name="filename"/>
        /// </summary>
        /// <param name="filename">File name with the extension</param>
        /// <param name="content"></param>
        public void WriteOtherFile(string filename, string content)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(filename, nameof(filename));
            try
            {
                File.WriteAllText(Path.Combine(defaultStoragePath, filename), content);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }
        /// <summary>
        /// Remove given file at <see cref="defaultStoragePath"/> directory with <see cref="defaultExtension"/> extension
        /// </summary>
        /// <param name="filename"></param>
        public void RemoveFile(string filename)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(filename, nameof(filename));
            try
            {
                File.Delete(Path.Combine(defaultStoragePath, Path.ChangeExtension(filename, defaultExtension)));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }
        /// <summary>
        /// Rename 
        /// </summary>
        /// <param name="old"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RenameFile(string old, string name)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(old, nameof(old));
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(name, nameof(name));

            try
            {
                old = Path.ChangeExtension(old, defaultExtension);
                name = Path.ChangeExtension(name, defaultExtension);

                File.Move(Path.Combine(defaultStoragePath, old), Path.Combine(defaultStoragePath, name));

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return false;
            }
        }
    }
}
