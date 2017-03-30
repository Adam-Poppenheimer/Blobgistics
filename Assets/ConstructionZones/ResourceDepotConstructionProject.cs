using System;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;
using Assets.Depots;
using Assets.Map;


namespace Assets.ConstructionZones {

    public class ResourceDepotConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        #region from ConstructionProjectBase

        public override ResourceSummary Cost {
            get { return _cost; }
        }
        [SerializeField] private ResourceSummary _cost;

        #endregion

        [SerializeField] private ResourceDepotFactoryBase DepotFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override void ExecuteBuild(MapNodeBase location) {
            DepotFactory.ConstructDepotAt(location);
        }

        #endregion

        #endregion

    }

}