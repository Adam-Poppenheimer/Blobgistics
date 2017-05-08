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

        public bool SiteContainsNecessaryResources;

        #endregion

        #region events

        public event EventHandler<EventArgs> BuildExecuted;
        public event EventHandler<EventArgs> SiteSet;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override bool IsValidAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override void ExecuteBuild(MapNodeBase location) {
            if(BuildExecuted != null) {
                BuildExecuted(this, EventArgs.Empty);
            }
        }

        public override bool BlobSiteContainsNecessaryResources(BlobSiteBase site) {
            return SiteContainsNecessaryResources;
        }

        public override void SetSiteForProject(BlobSiteBase site) {
            if(SiteSet != null) {
                SiteSet(this, EventArgs.Empty);
            }
        }

        public override string GetCostSummaryString() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
