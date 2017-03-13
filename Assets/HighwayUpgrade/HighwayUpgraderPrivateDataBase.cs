using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Highways;

namespace Assets.HighwayUpgrade {

    public abstract class HighwayUpgraderPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract BlobSiteBase UnderlyingSite { get; }
        public abstract BlobHighwayBase TargetedHighway { get; }
        public abstract BlobHighwayProfile ProfileToInsert { get; }
        public abstract HighwayUpgraderFactoryBase SourceFactory { get; }

        #endregion

    }

}
