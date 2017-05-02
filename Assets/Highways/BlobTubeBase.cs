using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    public abstract class BlobTubeBase : MonoBehaviour {

        #region instance fields and properties

        public abstract Vector3 SourceLocation { get; }
        public abstract Vector3 TargetLocation { get; }

        public abstract ReadOnlyCollection<ResourceBlobBase> Contents { get; }

        public abstract int Capacity { get; set; }
        public abstract int SpaceLeft { get; }

        public abstract float TransportSpeedPerSecond { get; set; }

        #endregion

        #region events

        public event EventHandler<BlobEventArgs> BlobReachedEndOfTube;

        protected void RaiseBlobReachedEndOfTube(ResourceBlobBase blob) {
            if(BlobReachedEndOfTube != null) {
                BlobReachedEndOfTube(this, new BlobEventArgs(blob));
            }
        }

        #endregion

        #region instance methods

        public abstract bool CanPushBlobInto(ResourceBlobBase blob);
        public abstract void PushBlobInto   (ResourceBlobBase blob);

        public abstract bool CanPullBlobFrom(ResourceBlobBase blob);
        public abstract void PullBlobFrom   (ResourceBlobBase blob);

        public abstract bool RemoveBlobFrom(ResourceBlobBase blob);

        public abstract void Clear();

        public abstract void SetPermissionForResourceType(ResourceType type, bool isPermitted);
        public abstract bool GetPermissionForResourceType(ResourceType type);

        public abstract void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation);

        #endregion

    }

}