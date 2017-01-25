using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

using Assets.BlobEngine;

namespace Assets.Editor {

    [ExecuteInEditMode]
    public static class EditorPrefabBuilder {

        #region static fields and properties


        public static BuildingPlotFactory PlotFactory {
            get {
                if(_plotFactory == null) {
                    throw new InvalidOperationException("PlotFactory is uninitialized");
                } else {
                    return _plotFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _plotFactory = value;
                }
            }
        }
        private static BuildingPlotFactory _plotFactory;

        public static ResourcePoolFactory PoolFactory {
            get {
                if(_poolFactory == null) {
                    throw new InvalidOperationException("PoolFactory is uninitialized");
                } else {
                    return _poolFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _poolFactory = value;
                }
            }
        }
        private static ResourcePoolFactory _poolFactory;

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

        public static Transform MapRoot {
            get {
                if(_mapRoot == null) {
                    throw new InvalidOperationException("MapRoot is uninitialized");
                } else {
                    return _mapRoot;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _mapRoot = value;
                }
            }
        }
        private static Transform _mapRoot;

        #endregion

        #region static methods

        [MenuItem("GameObject/Strategy Blobs/Building Plot", false, 10)]
        private static void BuildBuildingPlot(MenuCommand command) {
            HandleContext((PlotFactory.BuildBuildingPlot(Vector3.zero, MapRoot) as BuildingPlot).gameObject, command);
        }

        [MenuItem("GameObject/Strategy Blobs/Resource Pool", false, 10)]
        private static void BuildResourcePool(MenuCommand command) {
            HandleContext((PoolFactory.BuildResourcePool(Vector3.zero, MapRoot) as ResourcePool).gameObject, command);
        }

        private static void HandleContext(GameObject objectToManage, MenuCommand issuingCommand) {
            GameObjectUtility.SetParentAndAlign(objectToManage, issuingCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(objectToManage, "Create " + objectToManage.name);
            Selection.activeObject = objectToManage;
        }

        #endregion

    }

}
