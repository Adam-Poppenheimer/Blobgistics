using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;

using Assets.BlobSites;

namespace Assets.HighwayUpgraders {

    public abstract class HighwayUpgraderBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int ID { get; }

        public abstract BlobSiteBase UnderlyingSite { get; }

        public abstract BlobHighwayBase TargetedHighway { get; }

        public abstract BlobHighwayProfileBase ProfileToInsert { get; }

        #endregion

        #region instance methods

        public abstract ResourceSummary GetResourcesNeededToUpgrade();

        #endregion

    }

}
