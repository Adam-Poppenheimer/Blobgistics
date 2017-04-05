using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.Highways.ForTesting {

    public class MockResourceBlob : ResourceBlobBase {

        #region instance fields and properties

        #region from ResourceBlobBase

        public override ResourceType BlobType { get; set; }

        #endregion

        public List<MovementGoal> PushedGoals = new List<MovementGoal>();

        #endregion

        #region instance methods

        #region from ResourceBlobBase

        public override void PushNewMovementGoal(MovementGoal goal) {
            PushedGoals.Add(goal);
        }

        public override void Tick(float secondsPassed) {
            
        }

        #endregion

        #endregion
        

        
    }

}
