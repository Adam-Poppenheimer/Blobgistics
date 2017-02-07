using System;

using Assets.Map;

namespace Assets.BlobEngine {

    public interface IBlobSource : ITubableObject {
        
        #region properties

        MapNode Location { get; }

        #endregion

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