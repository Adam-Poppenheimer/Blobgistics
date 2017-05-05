using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;
using Assets.HighwayManager;

namespace Assets.ConstructionZones {

    public class HighwayManagerConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private IntResourceSummary Cost;

        [SerializeField] private HighwayManagerFactoryBase HighwayManagerFactory;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return Cost.IsContainedWithinBlobSite(site);
        }

        public override void ExecuteBuild(MapNodeBase location) {
            if(HighwayManagerFactory.CanConstructHighwayManagerAtLocation(location)) {
                HighwayManagerFactory.ConstructHighwayManagerAtLocation(location);
            }
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
