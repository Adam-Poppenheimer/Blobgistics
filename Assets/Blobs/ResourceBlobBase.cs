using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Blobs {

    public abstract class ResourceBlobBase : MonoBehaviour {

        #region static fields and properties

        public static readonly float DesiredZPositionOfAllBlobs = -2f;
        public static readonly float RadiusOfBlobs = 0.25f;

        protected static readonly float SecondsToPopIn = 0.25f;
        protected static readonly Vector3 StartingVelocity = new Vector3(5f, 5f, 5f);

        protected static readonly float DestinationSnapDelta = 0.01f;

        #endregion

        #region instance fields and properties

        public abstract ResourceType BlobType { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> BeingDestroyed;

        protected void RaiseBeingDestroyed() {
            if(BeingDestroyed != null) {
                BeingDestroyed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        public abstract void PushNewMovementGoal(MovementGoal goal);
        public abstract void ClearAllMovementGoals();

        public abstract void Tick(float secondsPassed);

        #endregion

    }

}
