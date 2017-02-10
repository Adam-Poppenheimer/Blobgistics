using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class TypeConstrainedBlobPile : BlobPileBase {

        #region instance fields and properties

        #region from BlobPileBase

        public override IEnumerable<ResourceBlob> Contents {
            get { return AllBlobs; }
        }

        #endregion

        private DictionaryOfLists<ResourceType, ResourceBlob> BlobsOfType =
            new DictionaryOfLists<ResourceType, ResourceBlob>();

        private List<ResourceBlob> AllBlobs =
            new List<ResourceBlob>();

        private Dictionary<ResourceType, int> CapacityByType = 
            new Dictionary<ResourceType, int>();

        #endregion

        #region constructors

        public TypeConstrainedBlobPile(Dictionary<ResourceType, int> capacityByType) {
            CapacityByType = new Dictionary<ResourceType, int>(capacityByType);
        }

        #endregion

        #region instance methods

        #region from BlobPileBase

        public override bool CanPlaceBlobInto(ResourceBlob blob) {
            return CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            int desiredCapacity = 0;
            CapacityByType.TryGetValue(type, out desiredCapacity);
            List<ResourceBlob> blobsOfRequestedType;
            BlobsOfType.TryGetValue(type, out blobsOfRequestedType);

            return blobsOfRequestedType != null && blobsOfRequestedType.Count < desiredCapacity;
        }

        public override void PlaceBlobInto(ResourceBlob blob) {
            if(CanPlaceBlobInto(blob)) {
                BlobsOfType.AddElementToList(blob.BlobType, blob);
                AllBlobs.Add(blob);
            }else {
                throw new BlobException("Cannot place this blob into this BlobPile");
            }
        }

        public override bool CanExtractAnyBlob() {
            return AllBlobs.Count > 0;
        }

        public override ResourceBlob ExtractAnyBlob() {
            if(CanExtractAnyBlob()) {
                var retval = AllBlobs.Last();
                BlobsOfType[retval.BlobType].Remove(retval);
                AllBlobs.Remove(retval);
                return retval;
            }else {
                throw new BlobException("Cannot extract any blob from this BlobPile");
            }
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            return BlobsOfType[type].Count > 0;
        }

        public override ResourceBlob ExtractBlobOfType(ResourceType type) {
            if(CanExtractBlobOfType(type)) {
                var retval = BlobsOfType[type].Last();
                BlobsOfType[type].Remove(retval);
                AllBlobs.Remove(retval);
                return retval;
            }else {
                throw new BlobException("Cannot extract a blob of this type from this BlobPile");
            }
        }

        public override bool TryExtractBlobFrom(ResourceBlob blob) {
            AllBlobs.Remove(blob);
            return BlobsOfType[blob.BlobType].Remove(blob);
        }

        public override void Clear() {
            AllBlobs.Clear();
            BlobsOfType.Clear();
        }

        public override IEnumerable<ResourceType> GetAllTypesWithin() {
            return BlobsOfType.Keys;
        }

        public override IEnumerable<ResourceBlob> GetAllBlobsOfType(ResourceType type) {
            List<ResourceBlob> blobsOfDesiredType;
            BlobsOfType.TryGetValue(type, out blobsOfDesiredType);
            return blobsOfDesiredType == null ? new List<ResourceBlob>() : blobsOfDesiredType;
        }

        public override int GetSpaceLeftForBlobOfType(ResourceType type) {
            int desiredCapacity = 0;
            CapacityByType.TryGetValue(type, out desiredCapacity);
            return desiredCapacity - GetAllBlobsOfType(type).Count();
        }

        public override Dictionary<ResourceType, IEnumerable<ResourceBlob>> GetAllBlobsOfAllTypes() {
            var retval = new Dictionary<ResourceType, IEnumerable<ResourceBlob>>();
            foreach(var resourceType in BlobsOfType.Keys) {
                retval[resourceType] = new List<ResourceBlob>(BlobsOfType[resourceType]);
            }
            return retval;
        }

        public override bool IsAtCapacity() {
            foreach(var resourceType in CapacityByType.Keys) {
                if(!BlobsOfType.ContainsKey(resourceType) || BlobsOfType[resourceType].Count < CapacityByType[resourceType]) {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #endregion

    }

}
