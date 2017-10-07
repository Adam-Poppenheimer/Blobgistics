using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.Core.ForTesting {

    public class MockResourceBlob : ResourceBlobBase {

        #region instance fields and properties

        #region from ResourceBlobBase

        public override ResourceType BlobType { get; set; }

        #endregion

        #endregion

        #region instance methods

        #region from ResourceBlobBase

        public override void ClearAllMovementGoals() {
            throw new NotImplementedException();
        }

        public override void EnqueueNewMovementGoal(MovementGoal goal) {
            throw new NotImplementedException();
        }

        public override void Tick(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }
}
