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

        [SerializeField] private BuildingPlotPrivateData GyserPrivateData;
        [SerializeField] private GameObject GyserPrefab;

        #endregion

        #region instance methods

        #region from IBuildingPlotFactory

        public override IBuildingPlot ConstructBuildingPlot(Vector3 localPosition, Transform parent) {
            var plotObject = GameObject.Instantiate(PlotPrefab);
            var plotBehaviour = plotObject.GetComponent<BuildingPlot>();
            if(plotBehaviour != null) {
                plotBehaviour.PrivateData = PlotPrivateData;
                plotBehaviour.transform.SetParent(parent);
                plotBehaviour.transform.localPosition = localPosition;
                plotBehaviour.Initialize();
            }else {
                throw new BlobException("The BuildingPlot prefab did not contain a BuildingPlot component");
            }
            return plotBehaviour;
        }

        public override IResourceGyser ConstructResourceGyser(Vector3 localPosition, Transform parent,
            ResourceType typeProduced) {
            
            var gyserObject = Instantiate(GyserPrefab);
            var gyserBehaviour = gyserObject.GetComponent<ResourceGyser>();
            if(gyserBehaviour != null) {
                gyserBehaviour.PrivateData = GyserPrivateData;
                gyserObject.transform.SetParent(parent);
                gyserObject.transform.localEulerAngles = localPosition;
            }else {
                throw new BlobException("The ResourceGyser prefab did not contain a ResourceGyser component");
            }
            return gyserBehaviour;
        }

        #endregion

        #endregion

    }

}
