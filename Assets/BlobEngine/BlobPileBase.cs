using System;
using System.Collections.Generic;

namespace Assets.BlobEngine {

    public abstract class BlobPileBase {

        #region instance fields and properties

        public abstract IEnumerable<ResourceBlob> Contents { get; }

        #endregion

        #region instance methods

        public abstract bool CanPlaceBlobInto      (ResourceBlob blob);
        public abstract bool CanPlaceBlobOfTypeInto(ResourceType type);
        public abstract void PlaceBlobInto         (ResourceBlob blob);    

        public abstract bool         CanExtractAnyBlob();
        public abstract ResourceBlob ExtractAnyBlob();

        public abstract bool         CanExtractBlobOfType(ResourceType type);
        public abstract ResourceBlob ExtractBlobOfType   (ResourceType type);
        public abstract bool         TryExtractBlobFrom  (ResourceBlob blob);

        public abstract void Clear();

        public abstract IEnumerable<ResourceType> GetAllTypesWithin();

        public abstract IEnumerable<ResourceBlob> GetAllBlobsOfType(ResourceType type);
        public abstract Dictionary<ResourceType, IEnumerable<ResourceBlob>> GetAllBlobsOfAllTypes();

        public abstract int GetSpaceLeftForBlobOfType(ResourceType type);

        public abstract bool IsAtCapacity();

        public bool ContainsResourceSummary(ResourceSummary summary) {
            return summary.IsContainedWithinBlobPile(this);
        }

        #endregion
                
    }

}