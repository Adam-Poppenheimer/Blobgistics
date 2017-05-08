using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;

namespace Assets.ConstructionZones {

    public class PlantForestConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private IntResourceSummary Cost;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override bool IsValidAtLocation(MapNodeBase location) {
            return location.Terrain == TerrainType.Grassland;
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        public override void ExecuteBuild(MapNodeBase location) {
            location.Terrain = TerrainType.Forest;
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            site.SetPlacementPermissionsAndCapacity(Cost);
        }

        public override string GetCostSummaryString() {
            return Cost.GetSummaryString();
        }

        #endregion

        #endregion
        
    }

}
