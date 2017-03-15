using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using Assets.Highways;

namespace Assets.HighwayUpgrade {

    public class HighwayUpgrader : HighwayUpgraderBase {

        #region instance fields and properties

        #region from HighwayUpgraderBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override BlobHighwayProfile ProfileToInsert {
            get { return PrivateData.ProfileToInsert; }
        }

        public override BlobHighwayBase TargetedHighway {
            get { return PrivateData.TargetedHighway; }
        }

        public override BlobSiteBase UnderlyingSite {
            get { return PrivateData.UnderlyingSite; }
        }

        #endregion

        public HighwayUpgraderPrivateDataBase PrivateData {
            get {
                if(_privateData == null) {
                    throw new InvalidOperationException("PrivateData is uninitialized");
                } else {
                    return _privateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                    _privateData.UnderlyingSite.BlobPlacedInto += UnderlyingSite_BlobPlacedInto;
                    UpdateUnderlyingSite();
                }
            }
        }

        private HighwayUpgraderPrivateDataBase _privateData;

        #endregion

        #region instance methods

        #region from HighwayUpgraderBase

        public override ResourceSummary GetResourcesNeededToUpgrade() {
            var countDict = new Dictionary<ResourceType, int>();
            foreach(var resourceType in ProfileToInsert.Cost) {
                countDict[resourceType] = ProfileToInsert.Cost[resourceType] - UnderlyingSite.GetCountOfContentsOfType(resourceType);
            }
            return new ResourceSummary(countDict);
        }

        #endregion

        private void UnderlyingSite_BlobPlacedInto(object sender, BlobEventArgs e) {
            if(ProfileToInsert.Cost.IsContainedWithinBlobSite(UnderlyingSite)) {
                UnderlyingSite.ClearContents();
                UnderlyingSite.ClearPermissionsAndCapacity();
                UnderlyingSite.TotalCapacity = 0;
                TargetedHighway.Profile = ProfileToInsert;
                PrivateData.SourceFactory.DestroyHighwayUpgrader(this);
            }
        }

        private void UpdateUnderlyingSite() {
            UnderlyingSite.ClearContents();
            UnderlyingSite.ClearPermissionsAndCapacity();
            UnderlyingSite.SetPlacementPermissionsAndCapacity(ProfileToInsert.Cost);
        }

        #endregion

    }

}
