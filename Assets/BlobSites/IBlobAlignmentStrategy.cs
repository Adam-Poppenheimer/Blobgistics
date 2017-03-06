using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites {

    public interface IBlobAlignmentStrategy {

        #region methods

        void RealignBlobs(IEnumerable<ResourceBlob> blobsToAlign, Vector2 centerPosition,
            float realignmentSpeedPerSecond);

        #endregion

    }

}
