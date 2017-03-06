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

        public abstract ReadOnlyCollection<ResourceBlob> BlobsWithin { get; }

        #endregion

        #region instance methods

        public abstract bool CanPushBlobInto(ResourceBlob blob);
        public abstract void PushBlobInto   (ResourceBlob blob);

        public abstract bool CanPullBlobFrom(ResourceBlob blob);
        public abstract void PullBlobFrom   (ResourceBlob blob);

        public abstract void SetPermissionForResourceType(ResourceType type, bool isPermitted);

        public abstract void TickMovement(float secondsPassed);

        public abstract void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation);

        #endregion

    }

}