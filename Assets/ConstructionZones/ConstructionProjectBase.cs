using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Map;

namespace Assets.ConstructionZones {

    public abstract class ConstructionProjectBase {

        #region instance fields and properties

        public abstract ResourceSummary Cost { get; }
        public abstract Action<MapNodeBase> BuildAction { get; }

        #endregion

    }

}
