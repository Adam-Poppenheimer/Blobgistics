using System;

using UnityEngine;

namespace Assets.Blobs {

    public struct MovementGoal  {

        #region instance fields and properties

        public readonly Vector3 DesiredLocation;
        public readonly float SpeedPerSecond;

        public readonly Action ActionToPerformOnTermination;

        #endregion

        #region constructors

        public MovementGoal(Vector3 desiredLocation, float speedPerSecond) : 
            this(desiredLocation, speedPerSecond, null) { }

        public MovementGoal(Vector3 desiredLocation, float speedPerSecond,
            Action actionToPerformOnTermination) {
            DesiredLocation = desiredLocation;
            SpeedPerSecond = speedPerSecond;
            ActionToPerformOnTermination = actionToPerformOnTermination;
        }

        #endregion

    }

}