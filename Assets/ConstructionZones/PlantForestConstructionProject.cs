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

    /// <summary>
    /// Defines and executes the planting of forests on grassland.
    /// </summary>
    public class PlantForestConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private IntPerResourceDictionary Cost;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        /// <inheritdoc/>
        public override bool IsValidAtLocation(MapNodeBase location) {
            return location.Terrain == TerrainType.Grassland;
        }

        /// <inheritdoc/>
        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        /// <inheritdoc/>
        public override void ExecuteBuild(MapNodeBase location) {
            location.Terrain = TerrainType.Forest;
        }

        /// <inheritdoc/>
        public override void SetSiteForProject(BlobSiteBase site) {
            site.SetPlacementPermissionsAndCapacity(Cost);
        }

        /// <inheritdoc/>
        public override ResourceDisplayInfo GetCostInfo() {
            return new ResourceDisplayInfo(Cost);
        }

        #endregion

        #endregion
        
    }

}
