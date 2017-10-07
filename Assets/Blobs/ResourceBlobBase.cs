using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Blobs {

    /// <summary>
    /// The abstract base class for all resource blobs, objects which are the
    /// products and needs of societies and whose manipulation represents the
    /// bulk of gameplay.
    /// </summary>
    public abstract class ResourceBlobBase : MonoBehaviour {

        //These static variables are almost certainly a poor way of handling configuration.
        //They were created very early in the development of the game, during prototyping,
        //and weren't re-analyzed until after production had ended.
        #region static fields and properties

        /// <summary>
        /// A common Z position that all blobs should occupy.
        /// </summary>
        public static readonly float DesiredZPositionOfAllBlobs = -2f;

        /// <summary>
        /// The radius that all blobs should have, used primarily by blob alignment strategies
        /// to organize blobs.
        /// </summary>
        public static readonly float RadiusOfBlobs = 0.25f;

        /// <summary>
        /// The amount of time it takes for a newly-created blob to pop into existence up to
        /// its normal size.
        /// </summary>
        protected static readonly float SecondsToPopIn = 0.25f;

        /// <summary>
        /// The starting velocity of a ResourceBlob's scale change, which determines how it
        /// pops into existence.
        /// </summary>
        protected static readonly Vector3 StartingScaleVelocity = new Vector3(5f, 5f, 5f);

        /// <summary>
        /// When a resource blob is within this distance of its destination, it should consider
        /// itself as having arrived and should snap to the location proper.
        /// </summary>
        protected static readonly float DestinationSnapDelta = 0.01f;

        #endregion

        #region instance fields and properties

        /// <summary>
        /// The type of blob this ResourceBlob is.
        /// </summary>
        public abstract ResourceType BlobType { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever the ResourceBlob is in the process of being destroyed, primarily
        /// used to help BlobTube remove references to blobs as they're being destroyed.
        /// </summary>
        public event EventHandler<EventArgs> BeingDestroyed;

        /// <summary>
        /// Used to signal the ResourceBlob's imminent demise.
        /// </summary>
        protected void RaiseBeingDestroyed() {
            if(BeingDestroyed != null) {
                BeingDestroyed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Adds the given movement goal to the end of its queue of pending movement goals.
        /// </summary>
        /// <param name="goal">The goal to be enqueued</param>
        public abstract void EnqueueNewMovementGoal(MovementGoal goal);

        /// <summary>
        /// Clears all pending movement goals
        /// </summary>
        public abstract void ClearAllMovementGoals();

        /// <summary>
        /// Advances the simulation of this ResourceBlob some number of seconds.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds that have passed since the last simulation update</param>
        public abstract void Tick(float secondsPassed);

        #endregion

    }

}
