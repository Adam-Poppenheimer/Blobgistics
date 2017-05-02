using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    public abstract class BlobSitePrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        public abstract float ConnectionCircleRadius { get; }

        public abstract BlobAlignmentStrategyBase AlignmentStrategy { get; }
        public abstract float BlobRealignmentSpeedPerSecond { get; }

        #endregion

    }

}
