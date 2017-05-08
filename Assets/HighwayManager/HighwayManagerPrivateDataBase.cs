using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Core;

namespace Assets.HighwayManager {

    public abstract class HighwayManagerPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract int NeedStockpileCoefficient { get; }

        public abstract float SecondsToPerformConsumption { get; }

        public abstract UIControlBase UIControl { get; }

        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        public abstract HighwayManagerFactoryBase ParentFactory { get; }

        public abstract IntResourceSummary EfficiencyGainFromResource { get; }

        #endregion

    }

}
