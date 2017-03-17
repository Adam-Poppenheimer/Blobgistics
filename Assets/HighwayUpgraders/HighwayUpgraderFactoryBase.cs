using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.BlobSites;

namespace Assets.HighwayUpgraders {

    public abstract class HighwayUpgraderFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract HighwayUpgraderBase GetHighwayUpgraderOfID(int id);

        public abstract bool HasUpgraderTargetingHighway(BlobHighwayBase highway);
        public abstract HighwayUpgraderBase GetUpgraderTargetingHighway(BlobHighwayBase highway);

        public abstract HighwayUpgraderBase BuildHighwayUpgrader(BlobHighwayBase targetedHighway, BlobSiteBase underlyingSite,
            BlobHighwayProfile profileToInsert);

        public abstract void DestroyHighwayUpgrader(HighwayUpgraderBase highwayUpgrader);

        #endregion

    }

}
