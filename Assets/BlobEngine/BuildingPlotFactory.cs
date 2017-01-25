using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    [ExecuteInEditMode]
    public class BuildingPlotFactory : IBuildingPlotFactory {

        #region instance fields and properties

        public UIFSM TopLevelUIFSM {
            get {
                if(_topLevelUIFSM == null) {
                    throw new InvalidOperationException("TopLevelUIFSM is uninitialized");
                } else {
                    return _topLevelUIFSM;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _topLevelUIFSM = value;
                }
            }
        }
        private UIFSM _topLevelUIFSM;

        public IBlobTubeFactory TubeFactory {
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
        private IBlobTubeFactory _tubeFactory;

        public GameObject PlotPrefab {
            get {
                if(_plotPrefab == null) {
                    throw new InvalidOperationException("PlotPrefab is uninitialized");
                } else {
                    return _plotPrefab;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _plotPrefab = value;
                }
            }
        }
        private GameObject _plotPrefab;

        #endregion

        #region constructors

        public BuildingPlotFactory() { }

        #endregion

        #region instance methods

        #region from IBuildingPlotFactory

        public IBuildingPlot BuildBuildingPlot(Vector3 localPosition, Transform parent) {
            var plotObject = GameObject.Instantiate(PlotPrefab);
            var plotBehaviour = plotObject.GetComponent<BuildingPlot>();
            if(plotBehaviour != null) {
                plotBehaviour.TopLevelUIFSM = TopLevelUIFSM;
                plotBehaviour.TubeFactory = TubeFactory;
                plotObject.transform.SetParent(parent);
                plotObject.transform.localPosition = localPosition;
            }else {
                throw new BlobException("The BuildingPlot prefab did not contain a BuildingPlot component");
            }
            return plotBehaviour;
        }

        #endregion

        #endregion
        
    }

}
