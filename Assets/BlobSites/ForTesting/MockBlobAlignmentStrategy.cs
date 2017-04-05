using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.BlobSites.ForTesting {

    public class MockBlobAlignmentStrategy : BlobAlignmentStrategyBase {

        #region instance fields and properties

        public List<ResourceBlobBase> LastAlignmentRequest = new List<ResourceBlobBase>();

        #endregion

        #region instance methods

        #region from BlobAlignmentStrategyBase

        public override void RealignBlobs(IEnumerable<ResourceBlobBase> blobsToAlign, Vector2 centerPosition, float realignmentSpeedPerSecond) {
            LastAlignmentRequest = new List<ResourceBlobBase>(blobsToAlign);
        }

        #endregion

        #endregion

    }

}
