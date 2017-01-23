using System;

using UnityEngine;

namespace Assets.BlobEngine {

    public interface IBlobTarget : ITubableObject {

        #region methods

        bool CanPlaceBlobOfTypeInto(ResourceType type);
        void PlaceBlobInto(ResourceBlob blob);

        void ReservePlaceForBlob(ResourceBlob blob);
        void UnreservePlaceForBlob(ResourceBlob blob);

        #endregion

    }

}