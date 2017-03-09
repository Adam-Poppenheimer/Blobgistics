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

        public abstract ReadOnlyCollection<ResourceBlob> Contents { get; }

        public abstract int Capacity { get; set; }
        public abstract int SpaceLeft { get; }

        public abstract float TransportSpeedPerSecond { get; set; }

        #endregion

        #region instance methods

        public abstract bool CanPushBlobInto(ResourceBlob blob);
        public abstract void PushBlobInto   (ResourceBlob blob);

        public abstract bool CanPullBlobFrom(ResourceBlob blob);
        public abstract void PullBlobFrom   (ResourceBlob blob);

        public abstract bool RemoveBlobFrom(ResourceBlob blob);

        public abstract void Clear();

        public abstract void SetPermissionForResourceType(ResourceType type, bool isPermitted);
        public abstract bool GetPermissionForResourceType(ResourceType type);

        public abstract void TickMovement(float secondsPassed);

        public abstract void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation);

        #endregion

    }

}