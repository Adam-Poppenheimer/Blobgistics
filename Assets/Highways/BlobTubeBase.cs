using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    /// <summary>
    /// The base class for blob tubes, which handle the transit of resources from
    /// one point to another.
    /// </summary>
    public abstract class BlobTubeBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The beginning of the tube, where pushed blobs must travel.
        /// </summary>
        public abstract Vector3 SourceLocation { get; }

        /// <summary>
        /// The end of the tube, where pulled blobs must be removed from.
        /// </summary>
        public abstract Vector3 TargetLocation { get; }

        /// <summary>
        /// The blobs within the tube.
        /// </summary>
        public abstract ReadOnlyCollection<ResourceBlobBase> Contents { get; }

        /// <summary>
        /// The maximum number of blobs that can be in the tube at any given time.
        /// </summary>
        public abstract int Capacity { get; set; }

        /// <summary>
        /// The number of blobs that can be placed into the tube until it becomes full.
        /// </summary>
        public abstract int SpaceLeft { get; }

        /// <summary>
        /// The speed at which blobs travel down the tube.
        /// </summary>
        public abstract float TransportSpeedPerSecond { get; set; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever a blob reaches the end of the tube.
        /// </summary>
        public event EventHandler<BlobEventArgs> BlobReachedEndOfTube;

        /// <summary>
        /// Fires a BlobReachedEndOfTube event.
        /// </summary>
        /// <param name="blob">The blob in question</param>
        protected void RaiseBlobReachedEndOfTube(ResourceBlobBase blob) {
            if(BlobReachedEndOfTube != null) {
                BlobReachedEndOfTube(this, new BlobEventArgs(blob));
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Determines whether the given blob can be pushed into the source of this tube.
        /// </summary>
        /// <param name="blob">The blob to consider</param>
        /// <returns>Whether the blob can be pushed into the tube</returns>
        public abstract bool CanPushBlobInto(ResourceBlobBase blob);

        /// <summary>
        /// Pushes the blob into the tube, giving it movement goals that bring it to the
        /// SourceLocation and then the TargetLocation of this tube.
        /// </summary>
        /// <param name="blob">The blob to push</param>
        public abstract void PushBlobInto   (ResourceBlobBase blob);

        /// <summary>
        /// Determines whether the given blob has traveled all the way through this tube
        /// and is ready to be pulled from it.
        /// </summary>
        /// <param name="blob">The blob to consider</param>
        /// <returns></returns>
        public abstract bool CanPullBlobFrom(ResourceBlobBase blob);

        /// <summary>
        /// Pulls a blob that has traveled all the way down this tube from the tube.
        /// </summary>
        /// <param name="blob">The blob to pull</param>
        public abstract void PullBlobFrom   (ResourceBlobBase blob);

        /// <summary>
        /// Removes the given blob from this tube regardless of its position.
        /// </summary>
        /// <param name="blob">The blob to remove</param>
        /// <returns></returns>
        public abstract bool RemoveBlobFrom(ResourceBlobBase blob);

        /// <summary>
        /// Removes all blobs from the tube.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Sets the pulling permission of this tube for the given ResourceType.
        /// </summary>
        /// <param name="type">The type to change</param>
        /// <param name="isPermitted">Whether it is now permitted</param>
        public abstract void SetPermissionForResourceType(ResourceType type, bool isPermitted);

        /// <summary>
        /// Gets the pulling permission of this tube for the given ResourceType.
        /// </summary>
        /// <param name="type">The type to consider</param>
        /// <returns></returns>
        public abstract bool GetPermissionForResourceType(ResourceType type);

        /// <summary>
        /// Changes the source and target locations of this tube and adjusts other
        /// elements of the tube accordingly.
        /// </summary>
        /// <param name="sourceLocation"></param>
        /// <param name="targetLocation"></param>
        public abstract void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation);

        #endregion

    }

}