using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;
using Assets.Session;

namespace Assets.Util {

    public class EditorWindowDependencyPusher : MonoBehaviour {

        #region static fields and properties

        public static BlobHighwayFactoryBase HighwayFactory {
            get { return StaticHighwayFactory; }
        }
        private static BlobHighwayFactoryBase StaticHighwayFactory;

        public static MapGraphBase MapGraph {
            get { return StaticMapGraph; }
        }
        private static MapGraphBase StaticMapGraph;

        public static SessionManager SessionManager {
            get { return StaticSessionManager; }
        }
        private static SessionManager StaticSessionManager;

        public static FileSystemLiaison FileSystemLiaison {
            get { return StaticFileSystemLiaison; }
        }
        private static FileSystemLiaison StaticFileSystemLiaison;

        #endregion

        #region instance fields and properties

        [SerializeField] private BlobHighwayFactoryBase highwayFactory;
        [SerializeField] private MapGraphBase mapGraph;
        [SerializeField] private SessionManager sessionManager;
        [SerializeField] private FileSystemLiaison fileSystemLiaison;

        #endregion

        #region instance methods

        #region Unity message methods

        private void OnValidate() {
            StaticHighwayFactory = highwayFactory;
            StaticMapGraph = mapGraph;
            StaticSessionManager = sessionManager;
            StaticFileSystemLiaison = fileSystemLiaison;
        }

        #endregion

        #endregion

    }

}
