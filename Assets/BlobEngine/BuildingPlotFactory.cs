using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using UnityCustomUtilities.UI;

using Assets.Map;

namespace Assets.BlobEngine {

    [ExecuteInEditMode]
    public class BuildingPlotFactory : BuildingPlotFactoryBase {

        #region instance fields and properties

        [SerializeField] private BuildingPlotPrivateData PlotPrivateData;
        [SerializeField] private GameObject PlotPrefab;

        #endregion

        #region instance methods

        #region from BuildingPlotFactoryBase

        public override IBuildingPlot ConstructBuildingPlot(MapNode location) {
            var plotObject = Instantiate(PlotPrefab, location.transform, false) as GameObject;
            var plotBehaviour = plotObject.GetComponent<BuildingPlot>();
            if(plotBehaviour != null) {
                plotBehaviour.PrivateData = PlotPrivateData;
                plotBehaviour.Location = location;
            }else {
                throw new BlobException("The BuildingPlot prefab did not contain a BuildingPlot component");
            }
            return plotBehaviour;
        }

        #endregion

        #endregion

    }

}
