using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;
using Assets.ResourceDepots;
using Assets.Map;
using Assets.BlobSites;

namespace Assets.ConstructionZones {

    public class ResourceDepotConstructionProject : FlexibleCostConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private ResourceDepotFactoryBase DepotFactory;

        [SerializeField] private List<TerrainType> PermittedTerrains;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override bool IsValidAtLocation(MapNodeBase location) {
            return PermittedTerrains.Contains(location.Terrain);
        }

        public override void ExecuteBuild(MapNodeBase location) {
            DepotFactory.ConstructDepotAt(location);
        }

        #endregion

        #endregion

    }

}