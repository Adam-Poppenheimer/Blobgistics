using System;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;
using Assets.Depots;
using Assets.Map;
using Assets.BlobSites;

namespace Assets.ConstructionZones {

    public class ResourceDepotConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private ResourceSummary Cost;

        [SerializeField] private ResourceDepotFactoryBase DepotFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override void ExecuteBuild(MapNodeBase location) {
            DepotFactory.ConstructDepotAt(location);
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            site.SetPlacementPermissionsAndCapacity(Cost);
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        #endregion

        #endregion

    }

}