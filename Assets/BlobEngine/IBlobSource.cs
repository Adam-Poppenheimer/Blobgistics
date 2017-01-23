using System;

using UnityEngine;

namespace Assets.BlobEngine {

    public interface IBlobSource : ITubableObject {

        #region events

        event EventHandler<BlobEventArgs> NewBlobAvailable;

        #endregion

        #region methods

        bool CanExtractAnyBlob();
        bool CanExtractBlobOfType(ResourceType type);

        ResourceType GetTypeOfNextExtractedBlob();

        ResourceBlob ExtractAnyBlob();
        ResourceBlob ExtractBlobOfType(ResourceType type);

        #endregion

    }

}