using System;

namespace Assets.BlobEngine {

    public interface IBlobSource {

        #region methods

        bool CanExtractAnyBlob();
        bool CanExtractBlobOfType(ResourceType type);

        ResourceType GetTypeOfNextExtractedBlob();

        ResourceBlob ExtractAnyBlob();
        ResourceBlob ExtractBlobOfType(ResourceType type);

        #endregion

    }

}