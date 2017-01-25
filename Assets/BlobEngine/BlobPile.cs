using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class BlobPile {

        #region instance fields and properties

        protected Dictionary<ResourceType, HashSet<ResourceBlob>> BlobsOfType;

        protected HashSet<ResourceBlob> AllBlobs;

        public IEnumerable<ResourceBlob> Blobs {
            get { return AllBlobs; }
        }
        
        public BlobPileCapacity Capacity {
            get { return _capacity; }
            set { _capacity = value; }
        }
        private BlobPileCapacity _capacity;

        public ResourceBlob LastBlobInserted {
            get { return _lastBlobInserted; }
            private set { _lastBlobInserted = value; }
        }
        private ResourceBlob _lastBlobInserted;

        #endregion

        #region constructors

        public BlobPile(BlobPileCapacity capacity){
            BlobsOfType = new Dictionary<ResourceType, HashSet<ResourceBlob>>();
            AllBlobs = new HashSet<ResourceBlob>();
            Capacity = capacity;
        }
        public BlobPile(BlobPile otherPile, BlobPileCapacity capacity){
            BlobsOfType = new Dictionary<ResourceType, HashSet<ResourceBlob>>(otherPile.BlobsOfType);
            AllBlobs = new HashSet<ResourceBlob>(otherPile.AllBlobs);
            Capacity = capacity;
        }

        #endregion

        #region instance methods

        public bool HasBlobsOfType(ResourceType type) {
            HashSet<ResourceBlob> desiredBlobs;
            BlobsOfType.TryGetValue(type, out desiredBlobs);
            return desiredBlobs != null && desiredBlobs.Count > 0;
        }

        public IEnumerable<ResourceBlob> GetBlobsOfType(ResourceType type) {
            HashSet<ResourceBlob> desiredBlobs;
            BlobsOfType.TryGetValue(type, out desiredBlobs);
            if(desiredBlobs == null) {
                return new List<ResourceBlob>();
            }
            return desiredBlobs;
        }

        public int GetCountOfBlobsOfType(ResourceType type) {
            HashSet<ResourceBlob> desiredBlobs;
            BlobsOfType.TryGetValue(type, out desiredBlobs);
            if(desiredBlobs == null) {
                return 0;
            }
            return desiredBlobs.Count;
        }

        public bool IsAtCapacity() {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(!IsAtCapacityForType(resourceType)) return false;
            }
            return true;
        }

        public bool IsAtCapacityForType(ResourceType type) {
            HashSet<ResourceBlob> blobsOfDesiredType;
            BlobsOfType.TryGetValue(type, out blobsOfDesiredType);
            int blobCountOfDesiredType = blobsOfDesiredType == null ? 0 : blobsOfDesiredType.Count;

            return Capacity.GetCapacityForType(type) <= blobCountOfDesiredType;
        }

        public bool CanInsertBlob(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            return !IsAtCapacityForType(blob.BlobType);
        }

        public bool CanInsertBlobOfType(ResourceType type) {
            return !IsAtCapacityForType(type);
        }

        public void InsertBlob(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }else if(CanInsertBlob(blob)) {
                HashSet<ResourceBlob> hashSetOfSimilarBlobs;
                BlobsOfType.TryGetValue(blob.BlobType, out hashSetOfSimilarBlobs);
                if(hashSetOfSimilarBlobs == null) {
                    hashSetOfSimilarBlobs = new HashSet<ResourceBlob>();
                    BlobsOfType[blob.BlobType] = hashSetOfSimilarBlobs;
                }
                hashSetOfSimilarBlobs.Add(blob);
                AllBlobs.Add(blob);
                LastBlobInserted = blob;
            }else {
                throw new BlobException("Inserting this blob into this BlobPile would cause the pile to exceed its capacity");
            }
        }

        public bool CanExtractBlobOfType(ResourceType type) {
            return GetCountOfBlobsOfType(type) > 0;
        }

        public bool CanExtractBlob(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            return AllBlobs.Contains(blob);
        }

        public ResourceBlob ExtractBlobOfType(ResourceType type) {
            if(CanExtractBlobOfType(type)) {
                var blobToExtract = BlobsOfType[type].Last();
                BlobsOfType[type].Remove(blobToExtract);
                AllBlobs.Remove(blobToExtract);
                return blobToExtract;
            }else {
                throw new BlobException("Cannot extract blob of type from this BlobPile");
            }
        }

        public void ExtractBlob(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }else if(CanExtractBlob(blob)) {
                BlobsOfType[blob.BlobType].Remove(blob);
                AllBlobs.Remove(blob);
            }else {
                throw new BlobException("Cannot extract this specific blob from this BlobPile");
            }
        }

        public bool TryExtractBlob(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            if(CanExtractBlob(blob)) {
                ExtractBlob(blob);
                return true;
            }else {
                return false;
            }
        }

        #endregion

    }

}
