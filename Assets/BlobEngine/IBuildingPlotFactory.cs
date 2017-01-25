using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public interface IBuildingPlotFactory {

        #region methods

        IBuildingPlot BuildBuildingPlot(Vector3 localPosition, Transform parent);

        #endregion

    }

}
