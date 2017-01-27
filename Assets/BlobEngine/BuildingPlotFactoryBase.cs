using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class BuildingPlotFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract IBuildingPlot BuildBuildingPlot(Vector3 localPosition, Transform parent);

        #endregion

    }

}
