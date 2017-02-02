using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Map;

namespace Assets.BlobEngine {

    public abstract class BlobTargetBehaviour : NodeOccupyingObject, IBlobTarget {

        #region instance fields and properties

        #region from ITubableObject

        public abstract Vector3 EastTubeConnectionPoint  { get; }
        public abstract Vector3 NorthTubeConnectionPoint { get; }
        public abstract Vector3 SouthTubeConnectionPoint { get; }
        public abstract Vector3 WestTubeConnectionPoint  { get; }

        #endregion

        protected BlobPileCapacity Capacity {
            get { return _capacity; }
            set {
                if(value == null){
                    throw new ArgumentNullException("Value");
                }
                _capacity = value;
                BlobsWithin.Capacity = _capacity;
                BlobsWithReservedPositions.Capacity = _capacity;
            }
        }
        private BlobPileCapacity _capacity = BlobPileCapacity.NoCapacity;

        protected BlobPile BlobsWithin;
        protected BlobPile BlobsWithReservedPositions;

        #endregion

        #region events

        public event EventHandler<BlobEventArgs> BlobInsertedInto;

        protected void RaiseBlobInsertedInto(ResourceBlob blobInserted) {
            if(BlobInsertedInto != null) {
                BlobInsertedInto(this, new BlobEventArgs(blobInserted));
            }
        }

        #endregion

        #region instance methods

        #region from ITubableObject

        public Vector3 GetConnectionPointInDirection(ManhattanDirection direction) {
            switch(direction) {
                case ManhattanDirection.North: return NorthTubeConnectionPoint;
                case ManhattanDirection.South: return SouthTubeConnectionPoint;
                case ManhattanDirection.East:  return EastTubeConnectionPoint;
                case ManhattanDirection.West:  return WestTubeConnectionPoint;
                default: return NorthTubeConnectionPoint;
            }
        }

        #endregion

        #region from IBlobTarget

        public virtual bool CanPlaceBlobOfTypeInto(ResourceType type) {
            return (
                BlobsWithin.GetCountOfBlobsOfType(type) + BlobsWithReservedPositions.GetCountOfBlobsOfType(type) < 
                BlobsWithin.Capacity.GetCapacityForType(type)
            ) && (
                BlobsWithin.CanInsertBlobOfType(type)
            );
        }

        public void PlaceBlobInto(ResourceBlob blob) {
            BlobsWithReservedPositions.TryExtractBlob(blob);
            if(CanPlaceBlobOfTypeInto(blob.BlobType)) {
                BlobsWithin.InsertBlob(blob);
                blob.transform.SetParent(this.transform, true);
                OnBlobPlacedInto(blob);
            }else {
                throw new BlobException("Cannot place this blob into this BlobTarget");
            }
        }

        public void ReservePlaceForBlob(ResourceBlob blob) {
            if(CanPlaceBlobOfTypeInto(blob.BlobType)) {
                BlobsWithReservedPositions.InsertBlob(blob);
            }else {
                throw new BlobException("Cannot reserve a place for a blob that is invalid for this BlobTarget");
            }
        }

        public void UnreservePlaceForBlob(ResourceBlob blob) {
            BlobsWithReservedPositions.TryExtractBlob(blob);
        }

        public void ClearAllBlobs(bool includeReserved = false) {
            var blobsToDestroy = new List<ResourceBlob>(BlobsWithin.Blobs);
            if(includeReserved) {
                blobsToDestroy.AddRange(BlobsWithReservedPositions.Blobs);
            }
            for(int i = blobsToDestroy.Count - 1; i >= 0; --i) {
                GameObject.Destroy(blobsToDestroy[i].gameObject);
            }
        }

        #endregion

        public void Initialize() {
            BlobsWithin = new BlobPile();
            BlobsWithin.Capacity = BlobPileCapacity.NoCapacity;
            BlobsWithReservedPositions = new BlobPile();
            BlobsWithReservedPositions.Capacity = BlobPileCapacity.NoCapacity;
            DoOnInitialize();
        }

        protected virtual void DoOnInitialize() { }

        protected virtual void OnBlobPlacedInto(ResourceBlob blobPlaced) { }

        #endregion
        
    }

}
