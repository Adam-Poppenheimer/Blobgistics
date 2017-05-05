using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

using Assets.Highways;
using Assets.Map;
using Assets.Societies;
using Assets.ResourceDepots;
using Assets.Generator;

namespace Assets.Editing {

    [ExecuteInEditMode]
    public static class EditorPrefabBuilder {

        #region static fields and properties

        public static MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        private static MapGraphBase _mapGraph;

        public static SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        private static SocietyFactoryBase _societyFactory;

        public static ResourceDepotFactoryBase ResourceDepotFactory {
            get { return _resourceDepotFactory; }
            set { _resourceDepotFactory = value; }
        }
        private static ResourceDepotFactoryBase _resourceDepotFactory;

        public static ResourceGeneratorFactory GeneratorFactory {
            get { return _generatorFactory; }
            set { _generatorFactory = value; }
        }
        private static ResourceGeneratorFactory _generatorFactory;

        #endregion

        #region static methods

        [MenuItem("Strategy Blobs/Construct Society At Location")]
        private static void ConstructSocietyAtLocation() {
            var locationToConstruct = Selection.activeTransform.GetComponent<MapNodeBase>();
            if(SocietyFactory.CanConstructSocietyAt(locationToConstruct, SocietyFactory.StandardComplexityLadder,
                SocietyFactory.DefaultComplexityDefinition)) {
                var newSociety = SocietyFactory.ConstructSocietyAt(locationToConstruct, SocietyFactory.StandardComplexityLadder, 
                    SocietyFactory.DefaultComplexityDefinition);
            }else {
                Debug.LogErrorFormat("Attempted to build a Society on Location {0}, but its construction was not permitted",
                    locationToConstruct.name);
            }
        }

        [MenuItem("Strategy Blobs/Construct Society At Location", true)]
        private static bool ValidateConstructSocietyAtLocation() {
            if(Selection.activeTransform != null) {
                var locationToBuild = Selection.activeTransform.GetComponent<MapNodeBase>();
                return locationToBuild != null && SocietyFactory.CanConstructSocietyAt(locationToBuild,
                    SocietyFactory.StandardComplexityLadder, SocietyFactory.DefaultComplexityDefinition);
            }else {
                return false;
            }
        }

        [MenuItem("Strategy Blobs/Construct Resource Depot At Location")]
        private static void ConstructResourceDepotAtLocation() {
            var locationToConstruct = Selection.activeTransform.GetComponent<MapNodeBase>();
            ResourceDepotFactory.ConstructDepotAt(locationToConstruct);
        }

        [MenuItem("Strategy Blobs/Construct Resource Depot At Location", true)]
        private static bool ValidateConstructResourceDepotAtLocation() {
            if(Selection.activeTransform != null) {
                var locationToBuild = Selection.activeTransform.GetComponent<MapNodeBase>();
                return locationToBuild != null && !ResourceDepotFactory.HasDepotAtLocation(locationToBuild);
            }else {
                return false;
            }
        }

        [MenuItem("Strategy Blobs/Construct Generator At Location")]
        private static void ConstructGeneratorAtLocation() {
            var locationToConstruct = Selection.activeTransform.GetComponent<MapNodeBase>();
            GeneratorFactory.ConstructGeneratorAtLocation(locationToConstruct);
        }

        [MenuItem("Strategy Blobs/Construct Generator At Location", true)]
        private static bool ValidateConstructGeneratorAtLocation() {
            if(Selection.activeTransform != null) {
                var locationToBuild = Selection.activeTransform.GetComponent<MapNodeBase>();
                return locationToBuild != null && GeneratorFactory.CanConstructGeneratorAtLocation(locationToBuild);
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
