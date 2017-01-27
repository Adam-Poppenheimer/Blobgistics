using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    [ExecuteInEditMode]
    public class BuildingPlotFactory : BuildingPlotFactoryBase {

        #region instance fields and properties

        [SerializeField] private BuildingPlotPrivateData PlotPrivateData;

        [SerializeField] private GameObject PlotPrefab;

        #endregion

        #region instance methods

        #region from IBuildingPlotFactory

        public override IBuildingPlot BuildBuildingPlot(Vector3 localPosition, Transform parent) {
            var plotObject = GameObject.Instantiate(PlotPrefab);
            var plotBehaviour = plotObject.GetComponent<BuildingPlot>();
            if(plotBehaviour != null) {
                plotBehaviour.PrivateData = PlotPrivateData;
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
