using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobSites;
using Assets.ConstructionZones;
using Assets.Map;

using Assets.UI.Blobs;

namespace Assets.Core.ForTesting {

    public class MockConstructionProject : ConstructionProjectBase {

        #region instance methods

        #region from ConstructionProjectBase

        public override bool IsValidAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            throw new NotImplementedException();
        }

        public override void ExecuteBuild(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override ResourceDisplayInfo GetCostInfo() {
            throw new NotImplementedException();
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
