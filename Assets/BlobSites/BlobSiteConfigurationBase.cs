using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    /// <summary>
    /// The abstract base class for all blob site configurations, which instruct blob sites
    /// where to destroy their blobs and how to organize their contents in space.
    /// </summary>
    public abstract class BlobSiteConfigurationBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The factory the blob site should use to destroy its blobs on a ClearBlobs operation.
        /// </summary>
        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        /// <summary>
        /// The radius of the circle that highways should connect to the edge of.
        /// </summary>
        public abstract float ConnectionCircleRadius { get; }

        /// <summary>
        /// The alignment strategy that will orchestrate blob movements for the blob site.
        /// </summary>
        public abstract BlobAlignmentStrategyBase AlignmentStrategy { get; }

        /// <summary>
        /// The speed at which blobs, when within the blob site, will realign themselves to
        /// the instructions of the alignment strategy.
        /// </summary>
        public abstract float BlobRealignmentSpeedPerSecond { get; }

        #endregion

    }

}
