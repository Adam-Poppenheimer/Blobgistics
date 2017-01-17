using System;

using UnityEngine;

namespace Assets.BlobEngine {

    public interface IBlobTarget : ITubableObject {

        #region properties

        Transform transform { get; }

        #endregion

        #region methods

        bool CanPlaceBlobOfTypeInto(ResourceType type);
        void PlaceBlobInto(ResourceBlob blob);

        #endregion

    }

}