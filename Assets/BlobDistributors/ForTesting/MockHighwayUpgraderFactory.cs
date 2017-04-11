using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Highways;
using Assets.HighwayUpgraders;

namespace Assets.BlobDistributors.ForTesting {

    public class MockHighwayUpgraderFactory : HighwayUpgraderFactoryBase {

        #region instance fields and properties

        public BlobHighwayProfile ProfileToUse {
            get {
                if(!hasLoadedProfile) {
                    var profileCost = ResourceSummary.BuildResourceSummary(
                        gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Food, 10));
                    _profileToUse = new BlobHighwayProfile(1f, 10, profileCost, 0.2f);
                    hasLoadedProfile = true;
                }
                return _profileToUse;
            }
        }
        private bool hasLoadedProfile = false;
        private BlobHighwayProfile _profileToUse;

        private Dictionary<BlobHighwayBase, MockHighwayUpgrader> UpgraderForHighway =
            new Dictionary<BlobHighwayBase, MockHighwayUpgrader>();

        #endregion

        #region instance methods

        #region from HighwayUpgraderFactoryBase

        public override HighwayUpgraderBase BuildHighwayUpgrader(BlobHighwayBase targetedHighway,
            BlobSiteBase underlyingSite, BlobHighwayProfile profileToInsert) {
            
            var hostingObject = new GameObject();
            var newUpgrader = hostingObject.AddComponent<MockHighwayUpgrader>();
            newUpgrader.SetUnderlyingSite(underlyingSite);
            underlyingSite.SetPlacementPermissionsAndCapacity(profileToInsert.Cost);

            UpgraderForHighway[targetedHighway] = newUpgrader;
            return newUpgrader;
        }

        public override void DestroyHighwayUpgrader(HighwayUpgraderBase highwayUpgrader) {
            throw new NotImplementedException();
        }

        public override HighwayUpgraderBase GetHighwayUpgraderOfID(int id) {
            throw new NotImplementedException();
        }

        public override HighwayUpgraderBase GetUpgraderTargetingHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        public override bool HasUpgraderTargetingHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
