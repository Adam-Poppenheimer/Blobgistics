using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    public class ClearForestConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private IntPerResourceDictionary Cost;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override bool IsValidAtLocation(MapNodeBase location) {
            return location.Terrain == TerrainType.Forest;
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        public override void ExecuteBuild(MapNodeBase location) {
            location.Terrain = TerrainType.Grassland;
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            site.SetPlacementPermissionsAndCapacity(Cost);
        }

        public override ResourceDisplayInfo GetCostInfo() {
            return new ResourceDisplayInfo(Cost);
        }

        #endregion

        #endregion
        
    }

}
