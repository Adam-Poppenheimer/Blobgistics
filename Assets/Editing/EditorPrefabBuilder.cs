using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

using Assets.BlobEngine;

namespace Assets.Editing {

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

        

        private static void HandleContext(GameObject objectToManage, MenuCommand issuingCommand) {
            GameObjectUtility.SetParentAndAlign(objectToManage, issuingCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(objectToManage, "Create " + objectToManage.name);
            Selection.activeObject = objectToManage;
        }

        #endregion

    }

}
