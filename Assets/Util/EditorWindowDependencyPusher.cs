using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;
using Assets.Session;

namespace Assets.Util {

    /// <summary>
    /// Makes certain objects in the scene available to the MapEditorWindow and its
    /// subsidiaries.
    /// </summary>
    public class EditorWindowDependencyPusher : MonoBehaviour {

        #region static fields and properties

        /// <summary>
        /// The HighwayFactory made available to EditorWindows.
        /// </summary>
        public static BlobHighwayFactoryBase HighwayFactory {
            get { return StaticHighwayFactory; }
        }
        private static BlobHighwayFactoryBase StaticHighwayFactory;

        /// <summary>
        /// The HighwayFactory made available to MapGraph.
        /// </summary>
        public static MapGraphBase MapGraph {
            get { return StaticMapGraph; }
        }
        private static MapGraphBase StaticMapGraph;

        /// <summary>
        /// The SessionManager made available to EditorWindows.
        /// </summary>
        public static SessionManagerBase SessionManager {
            get { return StaticSessionManager; }
        }
        private static SessionManagerBase StaticSessionManager;

        /// <summary>
        /// The FileSystemLiaison made available to EditorWindows.
        /// </summary>
        public static FileSystemLiaison FileSystemLiaison {
            get { return StaticFileSystemLiaison; }
        }
        private static FileSystemLiaison StaticFileSystemLiaison;

        #endregion

        #region instance fields and properties

        [SerializeField] private BlobHighwayFactoryBase highwayFactory;
        [SerializeField] private MapGraphBase mapGraph;
        [SerializeField] private SessionManagerBase sessionManager;
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
