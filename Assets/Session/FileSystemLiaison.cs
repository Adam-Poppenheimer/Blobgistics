using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;

using UnityEngine;

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

        public string SessionExtension {
            get { return _sessionExtension; }
        }
        [SerializeField] private string _sessionExtension;

        public string VictoryDataPath {
            get { return _victoryDataPath; }
        }
        [SerializeField] private string _victoryDataPath;

        public string VictoryDataExtension {
            get { return _victoryDataExtension; }
        }
        [SerializeField] private string _victoryDataExtension;

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

        #region instance methods

        public void WriteSavedGameToFile(SerializableSession session) {
            if(!loadedSavedGames.Contains(session)) {
                string path = string.Format("{0}/{1}/{2}.{3}", Application.persistentDataPath, SavedGameStoragePath,
                    session.Name, SessionExtension);
                WriteSessionToFile(session, path);
                loadedSavedGames.Add(session);
            }
        }

        public void DeleteSavedGame(string savedGameName) {
            var filesToDelete = SavedGameDirectory.GetFiles(savedGameName + "." + SessionExtension);
            foreach(var file in filesToDelete) {
                file.Delete();
            }
        }

        public void RefreshLoadedSavedGames() {
            if(string.IsNullOrEmpty(SavedGameStoragePath)) {
                throw new SessionException("Cannot refresh loaded saved games: SavedGameStoragePath must not be empty");
            }else if(string.IsNullOrEmpty(SessionExtension)) {
                throw new SessionException("Cannot refresh loaded saved games: SessionExtension must not be empty");
            }
            loadedSavedGames.Clear();
            SavedGameDirectory = Directory.CreateDirectory(Application.persistentDataPath + @"\" + SavedGameStoragePath);

            foreach(var file in SavedGameDirectory.GetFiles("*." + SessionExtension)) {
                if(file.Extension.Equals("." + SessionExtension)) {
                    loadedSavedGames.Add(LoadSessionFromFile(file));
                }
            }
        }

        public void WriteMapToFile(SerializableSession session) {
            if(!loadedMaps.Contains(session)) {
                string path = string.Format("{0}/{1}/{2}.{3}", Application.streamingAssetsPath, MapStoragePath,
                    session.Name, SessionExtension);
                WriteSessionToFile(session, path);
                loadedMaps.Add(session);
            }
        }

        public void DeleteMap(string mapName) {
            var filesToDelete = MapDirectory.GetFiles(mapName + "." + SessionExtension);
            foreach(var file in filesToDelete) {
                file.Delete();
            }
        }

        public void RefreshLoadedMaps() {
            if(string.IsNullOrEmpty(MapStoragePath)) {
                throw new SessionException("Cannot refresh loaded maps: MapStoragePath must not be empty");
            }else if(string.IsNullOrEmpty(SessionExtension)) {
                throw new SessionException("Cannot refresh loaded maps: SessionExtension must not be empty");
            }
            loadedMaps.Clear();
            MapDirectory = Directory.CreateDirectory(Application.streamingAssetsPath + @"\" + MapStoragePath);

            foreach(var file in MapDirectory.GetFiles("*." + SessionExtension)) {
                if(file.Extension.Equals("." + SessionExtension)) {
                    loadedMaps.Add(LoadSessionFromFile(file));
                }
            }
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
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, session);
            }
        }

        private SerializableSession LoadSessionFromFile(FileInfo file) {
            using(FileStream fileStream = file.OpenRead()) {
                try {
                    var formatter = new BinaryFormatter();
                    var session = formatter.Deserialize(fileStream) as SerializableSession;
                    if(session != null) {
                        return session;
                    }else {
                        throw new SessionException("Failed to load session from file: the file did not represent a SerializableSession object");
                    }
                }catch(SerializationException e) {
                    Debug.LogError("Failed to deserialize. Reason given: " + e.Message);
                    throw;
                }
            }
        }

        #endregion

    }

}
