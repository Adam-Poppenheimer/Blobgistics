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
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    public class FileSystemLiaison : MonoBehaviour {

        #region instance fields and properties        

        public string SavedGameStoragePath {
            get { return _savedGameStoragePath; }
        }
        [SerializeField] private string _savedGameStoragePath;

        public string MapStoragePath {
            get { return _mapStoragePath; }
        }
        [SerializeField] private string _mapStoragePath;

        public string VictoryDataPath {
            get { return _victoryDataPath; }
        }
        [SerializeField] private string _victoryDataPath;

        public string VictoryDataExtension {
            get { return _victoryDataExtension; }
        }
        [SerializeField] private string _victoryDataExtension;

        [SerializeField] private string RegistryAssetBundleName;

        public ReadOnlyCollection<SerializableSession> LoadedSavedGames {
            get { return loadedSavedGames.AsReadOnly(); }
        }
        private List<SerializableSession> loadedSavedGames = new List<SerializableSession>();

        public ReadOnlyCollection<SerializableSession> LoadedMaps {
            get { return loadedMaps.AsReadOnly(); }
        }
        private List<SerializableSession> loadedMaps = new List<SerializableSession>();

        private DirectoryInfo SavedGameDirectory;
        private DirectoryInfo MapDirectory;

        #endregion

        #region events

        public event EventHandler<SerializableSessionEventArgs> MapAsynchronouslyAdded;

        protected void RaiseMapAsynchronouslyAdded(SerializableSession session) {
            if(MapAsynchronouslyAdded != null) {
                MapAsynchronouslyAdded(this, new SerializableSessionEventArgs(session));
            }
        }

        #endregion

        #region instance methods

        public void WriteSavedGameToFile(SerializableSession session) {
            if(!loadedSavedGames.Contains(session)) {
                string path = string.Format("{0}/{1}/{2}.xml", Application.persistentDataPath, SavedGameStoragePath,
                    session.Name);
                WriteSessionToFile(session, path);
                loadedSavedGames.Add(session);
            }
        }

        public void DeleteSavedGame(string savedGameName) {
            var filesToDelete = SavedGameDirectory.GetFiles(savedGameName + ".xml");
            foreach(var file in filesToDelete) {
                file.Delete();
            }
        }

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

        public void WriteMapToFile(SerializableSession session) {
            if(!loadedMaps.Contains(session)) {
                string path = string.Format("{0}/{1}/{2}.xml", Application.streamingAssetsPath, MapStoragePath,
                    session.Name);
                WriteSessionToFile(session, path);
                loadedMaps.Add(session);
            }
        }

        public void DeleteMap(string mapName) {
            var filesToDelete = MapDirectory.GetFiles(mapName + ".xml");
            foreach(var file in filesToDelete) {
                file.Delete();
            }
        }

        public void RefreshLoadedMaps() {
            if(string.IsNullOrEmpty(MapStoragePath)) {
                throw new SessionException("Cannot refresh loaded maps: MapStoragePath must not be empty");
            }
            loadedMaps.Clear();
            if(Application.platform == RuntimePlatform.WebGLPlayer) {
                StartCoroutine(PerformMapRegistryTasks());
            }else {
                MapDirectory = Directory.CreateDirectory(Application.streamingAssetsPath + @"\" + MapStoragePath);

                foreach(var file in MapDirectory.GetFiles("*.xml")) {
                    if(file.Extension.Equals(".xml")) {
                        loadedMaps.Add(LoadSessionFromFile(file));
                    }
                }
            }
        }

        private IEnumerator PerformMapRegistryTasks() {
            var registryWWW = new WWW(string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, MapStoragePath, RegistryAssetBundleName));
            yield return registryWWW;
            var mapRegistry = registryWWW.assetBundle.LoadAsset("MapRegistry") as MapFileDirectoryRegistry;

            foreach(var mapName in mapRegistry.MapNamesWithExtensions) {
                StartCoroutine(PerformWWWMapDeserializationTasks(mapName));
            }
        }

        private IEnumerator PerformWWWMapDeserializationTasks(string mapName) {
            var mapWWW = new WWW(string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, MapStoragePath, mapName));
            yield return mapWWW;
            var sessionInFile = LoadSessionFromBytes(mapWWW.bytes);
            loadedMaps.Add(sessionInFile);
            RaiseMapAsynchronouslyAdded(sessionInFile);
        }

        public void WriteVictoryDataToFile(List<string> victoryData) {
            string fullPath = string.Format("{0}/{1}.{2}", Application.persistentDataPath, VictoryDataPath, VictoryDataExtension);

            using(FileStream fileStream = new FileStream(fullPath, FileMode.Create)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, victoryData);
            }
        }

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
