using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;

namespace Assets.ConstructionZones.ForTesting {

    public class MockConstructionProject : ConstructionProjectBase {

        #region instance fields and properties

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override void ExecuteBuild(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            throw new NotImplementedException();
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
