using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.BlobEngine {

    public interface IBlobTube {

        #region properties

        Vector3 LocationOfSourceToPullFrom { get; }
        Vector3 LocationOfTargetToPushTo   { get; }

        ReadOnlyCollection<ResourceBlob> BlobsWithin { get; }

        #endregion

        #region methods

        bool CanPushBlobInto(ResourceBlob blob);
        void PushBlobInto   (ResourceBlob blob);

        bool CanPullBlobFrom(ResourceBlob blob);
        void PullBlobFrom   (ResourceBlob blob);

        void TickMovement();

        void SetEndpoints(Vector3 sourceLocation, Vector3 targetLocation);

        #endregion

    }

}