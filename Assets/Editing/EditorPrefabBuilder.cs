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

        public static BlobGeneratorFactory GeneratorFactory {
            get {
                if(_generatorFactory == null) {
                    throw new InvalidOperationException("GeneratorFactory is uninitialized");
                } else {
                    return _generatorFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _generatorFactory = value;
                }
            }
        }
        private static BlobGeneratorFactory _generatorFactory;

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
            var newPlot = PlotFactory.ConstructBuildingPlot(Vector3.zero, MapRoot);
            HandleContext((newPlot as BuildingPlot).gameObject, command);
        }

        [MenuItem("GameObject/Strategy Blobs/Resource Pool", false, 10)]
        private static void BuildResourcePool(MenuCommand command) {
            var newPool = PoolFactory.BuildResourcePool(MapRoot, Vector3.zero);
            HandleContext((newPool as ResourcePool).gameObject, command);
        }

        [MenuItem("GameObject/Strategy Blobs/Resource Gyser", false, 10)]
        private static void BuildResourceGyser(MenuCommand command) {
            var newGyser = PlotFactory.ConstructResourceGyser(Vector3.zero, MapRoot, ResourceType.Red);
            HandleContext((newGyser as ResourceGyser).gameObject, command);
        }

        [MenuItem("GameObject/Strategy Blobs/Resource Generator (Red)")]
        private static void BuildGeneratorRed(MenuCommand command) {
            var newGenerator = GeneratorFactory.ConstructGenerator(MapRoot, Vector3.zero, ResourceType.Red);
            HandleContext((newGenerator as BlobGenerator).gameObject, command);
        }

        [MenuItem("GameObject/Strategy Blobs/Resource Generator (Green)")]
        private static void BuildGeneratorGreen(MenuCommand command) {
            var newGenerator = GeneratorFactory.ConstructGenerator(MapRoot, Vector3.zero, ResourceType.Green);
            HandleContext((newGenerator as BlobGenerator).gameObject, command);
        }

        [MenuItem("GameObject/Strategy Blobs/Resource Generator (Blue)")]
        private static void BuildGeneratorBlue(MenuCommand command) {
            var newGenerator = GeneratorFactory.ConstructGenerator(MapRoot, Vector3.zero, ResourceType.Blue);
            HandleContext((newGenerator as BlobGenerator).gameObject, command);
        }

        private static void HandleContext(GameObject objectToManage, MenuCommand issuingCommand) {
            GameObjectUtility.SetParentAndAlign(objectToManage, issuingCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(objectToManage, "Create " + objectToManage.name);
            Selection.activeObject = objectToManage;
        }

        #endregion

    }

}
