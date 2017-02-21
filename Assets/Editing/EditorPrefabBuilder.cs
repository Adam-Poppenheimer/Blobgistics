using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

using Assets.BlobEngine;
using Assets.Map;

namespace Assets.Editing {

    [ExecuteInEditMode]
    public static class EditorPrefabBuilder {

        #region static fields and properties

        public static BlobTubeFactory TubeFactory {
            get {
                if(_tubeFactory == null) {
                    throw new InvalidOperationException("TubeFactory is uninitialized");
                } else {
                    return _tubeFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _tubeFactory = value;
                }
            }
        }
        private static BlobTubeFactory _tubeFactory;

        public static MapGraph MapGraph {
            get {
                if(_mapGraph == null) {
                    throw new InvalidOperationException("MapGraph is uninitialized");
                } else {
                    return _mapGraph;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _mapGraph = value;
                }
            }
        }
        private static MapGraph _mapGraph;

        #endregion

        #region static methods

        [MenuItem("GameObject/Strategy Blobs/Map Node", false, 10)]
        private static void CreateMapNode(MenuCommand command) {
            Vector3 positionOfNode;
            var commandGameObject = command.context as GameObject;
            if(commandGameObject != null) {
                positionOfNode = commandGameObject.transform.position;
            }else {
                positionOfNode = Vector3.zero;
            }
            var newNode = MapGraph.BuildNode(positionOfNode);
            HandleContext(newNode.gameObject, command);
        }

        private static void HandleContext(GameObject objectToManage, MenuCommand issuingCommand) {
            Undo.RegisterCreatedObjectUndo(objectToManage, "Create " + objectToManage.name);
            Selection.activeObject = objectToManage;
        }

        #endregion

    }

}
