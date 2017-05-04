using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobSites;
using Assets.ConstructionZones;
using Assets.Map;

namespace Assets.Core.ForTesting {

    public class MockConstructionProject : ConstructionProjectBase {

        #region instance methods

        #region from ConstructionProjectBase

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            throw new NotImplementedException();
        }

        public override void ExecuteBuild(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override string GetCostSummaryString() {
            throw new NotImplementedException();
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
