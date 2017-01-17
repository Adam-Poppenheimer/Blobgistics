using System;

namespace Assets.BlobEngine {

    public interface IBlobTarget {

        #region methods

        bool CanPlaceBlobOfTypeInto(ResourceType type);
        void PlaceBlobInto(ResourceBlob blob);

        #endregion

    }

}