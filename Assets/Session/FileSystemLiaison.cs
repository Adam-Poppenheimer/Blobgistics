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

        public string SessionStoragePath {
            get { return _sessionStoragePath; }
        }
        [SerializeField] private string _sessionStoragePath;

        public string SaveFileExtension {
            get { return _saveFileExtension; }
        }
        [SerializeField] private string _saveFileExtension;

        public ReadOnlyCollection<SerializableSession> LoadedSessions {
            get { return loadedSessions.AsReadOnly(); }
        }
        private List<SerializableSession> loadedSessions = new List<SerializableSession>();

        private DirectoryInfo SessionDirectory;

        #endregion

        #region instance methods

        public void SaveSessionToFile(SerializableSession session) {
            string filePath = string.Format("{0}/{1}/{2}.{3}", Application.persistentDataPath, SessionStoragePath,
                session.Name, SaveFileExtension);

            using(FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, session);
                loadedSessions.Add(session);
            }
        }

        public void DeleteSessionFile(string sessionName) {
            var filesToDelete = SessionDirectory.GetFiles(sessionName + "." + SaveFileExtension);
            foreach(var file in filesToDelete) {
                file.Delete();
            }
        }

        public void RefreshLoadedSessions() {
            if(string.IsNullOrEmpty(SessionStoragePath)) {
                throw new SessionException("Cannot refresh loaded sessions: SessionStoragePath must not be empty");
            }else if(string.IsNullOrEmpty(SaveFileExtension)) {
                throw new SessionException("Cannot refresh loaded sessions: SaveFileExtension must not be empty");
            }
            loadedSessions.Clear();
            SessionDirectory = Directory.CreateDirectory(Application.persistentDataPath + @"\" + SessionStoragePath);

            foreach(var file in SessionDirectory.GetFiles("*." + SaveFileExtension)) {
                if(file.Extension.Equals("." + SaveFileExtension)) {
                    loadedSessions.Add(LoadSessionFromFile(file));
                }
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
