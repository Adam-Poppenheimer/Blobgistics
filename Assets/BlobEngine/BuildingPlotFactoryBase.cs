using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class BuildingPlotFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract IBuildingPlot ConstructBuildingPlot(Vector3 localPosition, Transform parent);
        public abstract IResourceGyser ConstructResourceGyser(Vector3 localPosition, Transform parent, ResourceType typeProduced);

        #endregion

    }

}
