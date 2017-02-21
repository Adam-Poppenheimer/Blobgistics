using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.BlobEngine {

    public interface IBlobTube {

        #region properties

        Vector3 SourceLocation { get; }
        Vector3 TargetLocation   { get; }

        ReadOnlyCollection<ResourceBlob> BlobsWithin { get; }

        #endregion

        #region methods

        bool CanPushBlobInto(ResourceBlob blob);
        void PushBlobInto   (ResourceBlob blob);

        bool CanPullBlobFrom(ResourceBlob blob);
        void PullBlobFrom   (ResourceBlob blob);

        void SetPermissionForResourceType(ResourceType type, bool isPermitted);

        void TickMovement(float secondsPassed);

        void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation);

        #endregion

    }

}