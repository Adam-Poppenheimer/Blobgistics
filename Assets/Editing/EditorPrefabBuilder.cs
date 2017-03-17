using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

using Assets.Highways;
using Assets.Map;
using Assets.Societies;

namespace Assets.Editing {

    [ExecuteInEditMode]
    public static class EditorPrefabBuilder {

        #region static fields and properties

        public static BlobTubeFactoryBase TubeFactory {
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
        private static BlobTubeFactoryBase _tubeFactory;

        public static MapGraphBase MapGraph {
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
        private static MapGraphBase _mapGraph;

        public static SocietyFactoryBase SocietyFactory {
            get {
                if(_societyFactory == null) {
                    throw new InvalidOperationException("SocietyFactory is uninitialized");
                } else {
                    return _societyFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _societyFactory = value;
                }
            }
        }
        private static SocietyFactoryBase _societyFactory;

        #endregion

        #region static methods

        [MenuItem("Strategy Blobs/Construct Society At Location")]
        private static void ConstructSocietyAtLocation() {
            var locationToConstruct = Selection.activeTransform.GetComponent<MapNodeBase>();
            if(SocietyFactory.CanConstructSocietyAt(locationToConstruct)) {
                var newSociety = SocietyFactory.ConstructSocietyAt(locationToConstruct, SocietyFactory.StandardComplexityLadder);
            }else {
                Debug.LogErrorFormat("Attempted to build a Society on Location {0}, but its construction was not permitted",
                    locationToConstruct.name);
            }
        }

        [MenuItem("Strategy Blobs/Construct Society At Location", true)]
        private static bool ValidateConstructSocietyAtLocation() {
            if(Selection.activeTransform != null) {
                var locationToBuild = Selection.activeTransform.GetComponent<MapNodeBase>();
                return locationToBuild != null && SocietyFactory.CanConstructSocietyAt(locationToBuild);
            }else {
                return false;
            }
        }

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
