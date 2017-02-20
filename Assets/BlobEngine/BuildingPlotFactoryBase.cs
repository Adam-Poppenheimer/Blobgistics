using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.BlobEngine {

    public abstract class BuildingPlotFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract IBuildingPlot ConstructBuildingPlot(MapNode location);

        #endregion

    }

}
