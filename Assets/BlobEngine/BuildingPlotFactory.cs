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

        [SerializeField] private BuildingPlotPrivateData GyserPrivateData;
        [SerializeField] private GameObject GyserPrefab;

        #endregion

        #region instance methods

        #region from IBuildingPlotFactory

        public override IBuildingPlot ConstructBuildingPlot(MapNode location) {
            var plotObject = GameObject.Instantiate(PlotPrefab);
            var plotBehaviour = plotObject.GetComponent<BuildingPlot>();
            if(plotBehaviour != null) {
                plotBehaviour.PrivateData = PlotPrivateData;
                plotBehaviour.transform.SetParent(location.transform);
                plotBehaviour.transform.localPosition = Vector3.zero;
                plotBehaviour.Initialize();
            }else {
                throw new BlobException("The BuildingPlot prefab did not contain a BuildingPlot component");
            }
            return plotBehaviour;
        }

        public override IResourceGyser ConstructResourceGyser(MapNode location, ResourceType typeProduced) {
            var gyserObject = Instantiate(GyserPrefab);
            var gyserBehaviour = gyserObject.GetComponent<ResourceGyser>();
            if(gyserBehaviour != null) {
                gyserBehaviour.PrivateData = GyserPrivateData;
                gyserObject.transform.SetParent(location.transform);
                gyserObject.transform.localEulerAngles = Vector3.zero;
            }else {
                throw new BlobException("The ResourceGyser prefab did not contain a ResourceGyser component");
            }
            return gyserBehaviour;
        }

        #endregion

        #endregion

    }

}
