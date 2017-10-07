using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;
using Assets.HighwayManager;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    /// <summary>
    /// Defines and executs the construction of a highway manager.
    /// </summary>
    public class HighwayManagerConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        /// <summary>
        /// The fixed cost of the project.
        /// </summary>
        [SerializeField] private IntPerResourceDictionary Cost;

        /// <summary>
        /// The factory that should be used to construct the highway manager.
        /// </summary>
        [SerializeField] private HighwayManagerFactoryBase HighwayManagerFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        /// <inheritdoc/>
        public override bool IsValidAtLocation(MapNodeBase location) {
            return location.Terrain != TerrainType.Mountains;
        }

        /// <inheritdoc/>
        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        /// <inheritdoc/>
        public override void ExecuteBuild(MapNodeBase location) {
            if(HighwayManagerFactory.CanConstructHighwayManagerAtLocation(location)) {
                HighwayManagerFactory.ConstructHighwayManagerAtLocation(location);
            }
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
