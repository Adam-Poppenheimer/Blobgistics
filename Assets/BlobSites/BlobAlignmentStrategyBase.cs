using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    public abstract class BlobAlignmentStrategyBase : MonoBehaviour {

        #region methods

        public abstract void RealignBlobs(IEnumerable<ResourceBlob> blobsToAlign, Vector2 centerPosition,
            float realignmentSpeedPerSecond);

        #endregion

    }

}
