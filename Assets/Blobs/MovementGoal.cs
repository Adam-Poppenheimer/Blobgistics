using System;

using UnityEngine;

namespace Assets.Blobs {

    /// <summary>
    /// POD class for recording the movement goals of ResourceBlobs.
    /// </summary>
    public struct MovementGoal {

        #region instance fields and properties

        /// <summary>
        /// The location the ResourceBlob should reach.
        /// </summary>
        public readonly Vector3 DesiredLocation;

        /// <summary>
        /// The speed the ResourceBlob is instructed to travel at.
        /// </summary>
        public readonly float SpeedPerSecond;

        /// <summary>
        /// The action that the ResourceBlob should call when it's reached its destination.
        /// </summary>
        public readonly Action ActionToPerformOnTermination;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new movement goal to reach a desired location at a desired speed.
        /// </summary>
        /// <param name="desiredLocation">The location to travel to</param>
        /// <param name="speedPerSecond">The speed at which the ResourceBlob should travel</param>
        public MovementGoal(Vector3 desiredLocation, float speedPerSecond) : 
            this(desiredLocation, speedPerSecond, null) { }

        /// <summary>
        /// Creates a new movement goal to reach a desired location at a desired speed, and an action
        /// to be performed on arrival.
        /// </summary>
        /// <param name="desiredLocation">The location to travel to</param>
        /// <param name="speedPerSecond">The speed at which the ResourceBlob should travel</param>
        /// <param name="actionToPerformOnTermination">The action that should be called when the ResourceBlob reaches its destination</param>
        public MovementGoal(Vector3 desiredLocation, float speedPerSecond,
            Action actionToPerformOnTermination) {
            DesiredLocation = desiredLocation;
            SpeedPerSecond = speedPerSecond;
            ActionToPerformOnTermination = actionToPerformOnTermination;
        }

        #endregion

    }

}