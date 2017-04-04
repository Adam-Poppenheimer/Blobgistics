using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using Assets.Highways;
using Assets.HighwayUpgraders;

namespace Assets.BlobDistributors.ForTesting {

    public class MockHighwayUpgrader : HighwayUpgraderBase {

        #region instance fields and properties

        #region from HighwayUpgraderBase

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobHighwayProfile ProfileToInsert {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobHighwayBase TargetedHighway {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobSiteBase UnderlyingSite {
            get { return _underlyingSite; }
        }
        public void SetUnderlyingSite(BlobSiteBase value) {
            _underlyingSite = value;
        }
        private BlobSiteBase _underlyingSite;

        #endregion

        #endregion

        #region instance methods

        #region from HighwayUpgraderBase

        public override ResourceSummary GetResourcesNeededToUpgrade() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
