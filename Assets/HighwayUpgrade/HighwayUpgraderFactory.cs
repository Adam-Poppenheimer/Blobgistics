using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Highways;

namespace Assets.HighwayUpgrade {

    public class HighwayUpgraderFactory : HighwayUpgraderFactoryBase {

        #region instance methods

        #region from HighwayUpgraderFactoryBase

        public override HighwayUpgraderBase BuildHighwayUpgrader(BlobHighwayBase targetedHighway, BlobSiteBase underlyingSite,
            BlobHighwayProfile profileToInsert) {
            var hostingObject = new GameObject();

            var privateData = hostingObject.AddComponent<HighwayUpgraderPrivateData>();
            privateData.SetTargetedHighway(targetedHighway);
            privateData.SetUnderlyingSite(underlyingSite);
            privateData.SetProfileToInsert(profileToInsert);
            privateData.SetSourceFactory(this);

            var newUpgrader = hostingObject.AddComponent<HighwayUpgrader>();
            newUpgrader.PrivateData = privateData;

            return newUpgrader;
        }

        public override void DestroyHighwayUpgrader(HighwayUpgraderBase highwayUpgrader) {
            DestroyImmediate(highwayUpgrader.gameObject);
        }

        #endregion

        #endregion

    }

}
