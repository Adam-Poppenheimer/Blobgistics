using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    /// <summary>
    /// The abstract base class for all blob alignment strategies, which manage the
    /// positioning of resource blobs contained within a BlobSiteBase.
    /// </summary>
    public abstract class BlobAlignmentStrategyBase : MonoBehaviour {

        #region methods

        /// <summary>
        /// Realigns the specified blobs around the given centerPosition by enqueueing new movement
        /// goals to various locations.
        /// </summary>
        /// <param name="blobsToAlign">All the blobs that must be aligned</param>
        /// <param name="centerPosition">The center position from which the alignment will be determined</param>
        /// <param name="realignmentSpeedPerSecond">How fast the blobs will move to their assigned locations</param>
        public abstract void RealignBlobs(IEnumerable<ResourceBlobBase> blobsToAlign, Vector2 centerPosition,
            float realignmentSpeedPerSecond);

        #endregion

    }

}
