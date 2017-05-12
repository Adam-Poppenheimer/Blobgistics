using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map.ForTesting {

    public class MockBlobSiteFactory : BlobSiteFactoryBase {

        #region instance methods

        #region from BlobSiteFactoryBase

        public override BlobSiteBase ConstructBlobSite(GameObject hostingObject) {
            return hostingObject.AddComponent<MockBlobSite>();
        }

        #endregion

        #endregion
        
    }

}
