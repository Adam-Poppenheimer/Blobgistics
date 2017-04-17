using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Highways;
using Assets.Core;

namespace Assets.HighwayUpgraders {

    public abstract class HighwayUpgraderPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract BlobSiteBase UnderlyingSite { get; }

        public abstract BlobHighwayBase TargetedHighway { get; }

        public abstract BlobHighwayProfileBase ProfileToInsert { get; }

        public abstract HighwayUpgraderFactoryBase SourceFactory { get; }

        public abstract UIControlBase UIControl { get; }

        #endregion

    }

}
