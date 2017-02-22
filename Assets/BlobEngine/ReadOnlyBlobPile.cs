using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BlobEngine {

    public class ReadOnlyBlobPile {

        #region instance fields and properties

        public IEnumerable<ResourceBlob> Contents {
            get { return InnerBlobPile.Contents; }
        }

        private BlobPileBase InnerBlobPile;

        #endregion

        #region constructors

        public ReadOnlyBlobPile(BlobPileBase innerBlobPile) {
            InnerBlobPile = innerBlobPile;
        }

        #endregion

        #region instance methods

        public bool CanPlaceBlobInto(ResourceBlob blob) {
            return InnerBlobPile.CanPlaceBlobInto(blob);
        }

        public bool CanPlaceBlobOfTypeInto(ResourceType type) {
            return InnerBlobPile.CanPlaceBlobOfTypeInto(type);
        }

        public bool CanExtractAnyBlob() {
            return InnerBlobPile.CanExtractAnyBlob();
        }

        public bool CanExtractBlobOfType(ResourceType type) {
            return InnerBlobPile.CanExtractBlobOfType(type);
        }

        public IEnumerable<ResourceType> GetAllTypesWithin() {
            return InnerBlobPile.GetAllTypesWithin();
        }

        public IEnumerable<ResourceBlob> GetAllBlobsOfType(ResourceType type) {
            return InnerBlobPile.GetAllBlobsOfType(type);
        }

        public Dictionary<ResourceType, IEnumerable<ResourceBlob>> GetAllBlobsOfAllTypes() {
            return InnerBlobPile.GetAllBlobsOfAllTypes();
        }

        public int GetSpaceLeftForBlobOfType(ResourceType type) {
            return InnerBlobPile.GetSpaceLeftForBlobOfType(type);
        }

        public bool IsAtCapacity() {
            return InnerBlobPile.IsAtCapacity();
        }

        public bool ContainsResourceSummary(ResourceSummary summary) {
            return InnerBlobPile.ContainsResourceSummary(summary);
        }

        #endregion

    }

}
