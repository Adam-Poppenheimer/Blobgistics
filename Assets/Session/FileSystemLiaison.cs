using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    /// <summary>
    /// A class that handles all interaction between the rest of the game and the file system.
    /// </summary>
    /// <remarks>
    /// Saved games and maps are both stored as SerializableSession objects written to an XML
    /// file via a DataContractSerializer.
    /// 
    /// All paths are relative to either Application.persistentDataPath or Application.streamingAssetsPath.
    /// 
    /// This class is the main reason why this game does not deploy as a web application very
    /// well. If you wanted to make it compatible with browsers, you'd need to change
    /// the way FileSystemLiaison implements persistence.
    /// </remarks>
    public class FileSystemLiaison : MonoBehaviour {

        #region instance fields and properties        

        /// <summary>
        /// The path to all saved games, relative to <see cref="Application.persistentDataPath"/>.
        /// </summary>
        public string SavedGameStoragePath {
            get { return _savedGameStoragePath; }
        }
        [SerializeField] private string _savedGameStoragePath;

        /// <summary>
        /// The path to all stored maps, relative to <see cref="Application.streamingAssetsPath"/>.
        /// </summary>
        public string MapStoragePath {
            get { return _mapStoragePath; }
        }
        [SerializeField] private string _mapStoragePath;

        /// <summary>
        /// The path to all victory data, relative to <see cref="Application.persistentDataPath"/>.
        /// </summary>
        public string VictoryDataPath {
            get { return _victoryDataPath; }
        }
        [SerializeField] private string _victoryDataPath;

        /// <summary>
        /// The extension to the victory data file.
        /// </summary>
        public string VictoryDataExtension {
            get { return _victoryDataExtension; }
        }
        [SerializeField] private string _victoryDataExtension;

        /// <summary>
        /// All of the saved games that this FileSystemLiaison has loaded and are ready to use.
        /// </summary>
        public ReadOnlyCollection<SerializableSession> LoadedSavedGames {
            get { return loadedSavedGames.AsReadOnly(); }
        }
        private List<SerializableSession> loadedSavedGames = new List<SerializableSession>();

        /// <summary>
        /// All of the maps that this FileSystemLiaison has loaded and are ready to use.
        /// </summary>
        public ReadOnlyCollection<SerializableSession> LoadedMaps {
            get { return loadedMaps.AsReadOnly(); }
        }
        private List<SerializableSession> loadedMaps = new List<SerializableSession>();

        private DirectoryInfo SavedGameDirectory;
        private DirectoryInfo MapDirectory;

        #endregion

        #region instance methods

        /// <summary>
        /// Writes the given session to a file.
        /// </summary>
        /// <remarks>
        /// The file is stored in the SavedGamesStoragePath, which is relative
        /// to Application.persistentDataPath. The name of the file is the name of the
        /// session.
        /// </remarks>
        /// <param name="session"></param>
        public void WriteSavedGameToFile(SerializableSession session) {
            if(!loadedSavedGames.Contains(session)) {
                string path = string.Format("{0}/{1}/{2}.xml", Application.persistentDataPath, SavedGameStoragePath,
                    session.Name);
                WriteSessionToFile(session, path);
                loadedSavedGames.Add(session);
            }
        }

        /// <summary>
        /// Deletes the saved game file with the given name.
        /// </summary>
        /// <param name="savedGameName">The name of the file to remove without the extension</param>
        public void DeleteSavedGame(string savedGameName) {
            var filesToDelete = SavedGameDirectory.GetFiles(savedGameName + ".xml");
            foreach(var file in filesToDelete) {
                file.Delete();
            }
        }

        /// <summary>
        /// Refreshes all of the loaded saved games, searching for files in
        /// the SavedGameStoragePath and using them to populate the LoadedSavedGames property.
        /// </summary>
        public void RefreshLoadedSavedGames() {
            if(string.IsNullOrEmpty(SavedGameStoragePath)) {
                throw new SessionException("Cannot refresh loaded saved games: SavedGameStoragePath must not be empty");
            }
            loadedSavedGames.Clear();
            SavedGameDirectory = Directory.CreateDirectory(Application.persistentDataPath + @"\" + SavedGameStoragePath);

            foreach(var file in SavedGameDirectory.GetFiles("*.xml")) {
                if(file.Extension.Equals(".xml")) {
                    loadedSavedGames.Add(LoadSessionFromFile(file));
                }
            }
        }

        /// <summary>
        /// Writes the given session as a map to a file.
        /// </summary>
        /// <remarks>
        /// The file is stored in the MapStoragePath, which is relative to <see cref="Application.streamingAssetsPath"/>.
        /// The name of the file is the name of the session.
        /// </remarks>
        /// <param name="session">The session to write as a map</param>
        public void WriteMapToFile(SerializableSession session) {
            if(!loadedMaps.Contains(session)) {
                string path = string.Format("{0}/{1}/{2}.xml", Application.streamingAssetsPath, MapStoragePath,
                    session.Name);
                WriteSessionToFile(session, path);
                loadedMaps.Add(session);
            }
        }

        /// <summary>
        /// Deletes the map of the given name from the map storage directory.
        /// </summary>
        /// <param name="mapName"></param>
        public void DeleteMap(string mapName) {
            var filesToDelete = MapDirectory.GetFiles(mapName + ".xml");
            foreach(var file in filesToDelete) {
                file.Delete();
            }
        }

        /// <summary>
        /// Refreshes all loaded maps, searching for files in the map storage directory
        /// and using them to populate the LoadedMaps property.
        /// </summary>
        public void RefreshLoadedMaps() {
            if(string.IsNullOrEmpty(MapStoragePath)) {
                throw new SessionException("Cannot refresh loaded maps: MapStoragePath must not be empty");
            }
            loadedMaps.Clear();
            MapDirectory = Directory.CreateDirectory(Application.streamingAssetsPath + @"\" + MapStoragePath);

            foreach(var file in MapDirectory.GetFiles("*.xml")) {
                if(file.Extension.Equals(".xml")) {
                    loadedMaps.Add(LoadSessionFromFile(file));
                }
            }
        }

        /// <summary>
        /// Writes victory data, consisting of a series of map names, to the victory
        /// data file, so that future executions of the program know which maps have
        /// been beaten on this computer.
        /// </summary>
        /// <remarks>
        /// The victory data file is located at the VictoryDataPath, which is relative
        /// to Application.persistentDataPath.
        /// </remarks>
        /// <param name="victoryData"></param>
        public void WriteVictoryDataToFile(List<string> victoryData) {
            string fullPath = string.Format("{0}/{1}.{2}", Application.persistentDataPath, VictoryDataPath, VictoryDataExtension);

            using(FileStream fileStream = new FileStream(fullPath, FileMode.Create)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, victoryData);
            }
        }

        /// <summary>
        /// Reads victory data, consisting of a series of map names, from the
        /// victory data file, and returns the data.
        /// </summary>
        /// <remarks>
        /// The victory data file is located at the VictoryDataPath, which is relative
        /// to Application.persistentDataPath.
        /// </remarks>
        /// <returns>A list of map names that have been cleared on this system</returns>
        public List<string> ReadVictoryDataFromFile() {
            string fullPath = string.Format("{0}/{1}.{2}", Application.persistentDataPath, VictoryDataPath, VictoryDataExtension);
            if(!File.Exists(fullPath)) {
                WriteVictoryDataToFile(new List<string>());
            }
            using(FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate)) {
                try {
                    var formatter = new BinaryFormatter();
                    var victoryData = formatter.Deserialize(fileStream) as List<string>;
                    return victoryData;
                }catch(SerializationException e) {
                    Debug.LogError("Failed to deserialize. Reason given: " + e.Message);
                    throw;
                }
            }
        }

        private void WriteSessionToFile(SerializableSession session, string path) {
            using(FileStream fileStream = new FileStream(path, FileMode.Create)) {
                using(var xmlWriter = XmlDictionaryWriter.CreateTextWriter(fileStream)) {
                    var knownTypes = new List<Type> {
                        typeof(Dictionary<ResourceType, bool>), typeof(Dictionary<ResourceType, int>),
                        typeof(SerializableVector3)
                    };
                
                    var contractSerializer = new DataContractSerializer(typeof(SerializableSession), knownTypes);
                    contractSerializer.WriteObject(xmlWriter, session);
                }
            }
        }

        private SerializableSession LoadSessionFromFile(FileInfo file) {
            using(FileStream fileStream = file.OpenRead()) {
                using(var xmlReader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas())) {
                    return LoadSession(xmlReader);
                }
            }
        }

        private SerializableSession LoadSessionFromBytes(Byte[] bytes) {
            using(var xmlReader = XmlDictionaryReader.CreateTextReader(bytes, XmlDictionaryReaderQuotas.Max)) {
                return LoadSession(xmlReader);
            }
        }

        private SerializableSession LoadSession(XmlDictionaryReader xmlReader) {
            try {
                var knownTypes = new List<Type> {
                    typeof(Dictionary<ResourceType, bool>), typeof(Dictionary<ResourceType, int>),
                    typeof(SerializableVector3)
                };

                var contractSerializer = new DataContractSerializer(typeof(SerializableSession), knownTypes);
                var sessionInFile = (SerializableSession)contractSerializer.ReadObject(xmlReader, true);
                return sessionInFile;
            }catch(SerializationException e) {
                Debug.LogError("Failed to deserialize. Reason given: " + e.Message);
                throw;
            }
        }

        #endregion

    }

}
