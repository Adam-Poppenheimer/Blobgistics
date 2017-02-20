using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Map;


namespace Assets.BlobEngine {

    public abstract class BlobSiteBase : NodeOccupyingObject, IBlobSite {

        #region instance fields and properties

        #region from IBlobSite

        public abstract bool AcceptsExtraction { get; }
        public abstract bool AcceptsPlacement  { get; }

        public abstract Vector3 NorthTubeConnectionPoint { get; }
        public abstract Vector3 SouthTubeConnectionPoint { get; }
        public abstract Vector3 EastTubeConnectionPoint  { get; }
        public abstract Vector3 WestTubeConnectionPoint  { get; }      

        #endregion

        protected abstract BlobPileBase BlobsWithin { get; }
        protected abstract BlobPileBase BlobsWithReservedPositions { get; }

        #endregion

        #region events

        #region from IBlobSite

        public event EventHandler<BlobEventArgs> BlobPlacedInto;
        public event EventHandler<BlobEventArgs> BlobExtractedFrom;
        public event EventHandler<EventArgs>     AllBlobsCleared;

        protected void RaiseBlobPlacedInto   (ResourceBlob blob) { RaiseEvent(BlobPlacedInto,    new BlobEventArgs(blob)); }
        protected void RaiseBlobExtractedFrom(ResourceBlob blob) { RaiseEvent(BlobExtractedFrom, new BlobEventArgs(blob)); }
        protected void RaiseAllBlobsCleared  ()                  { RaiseEvent(AllBlobsCleared,       EventArgs.Empty    ); }

        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs {
            if(handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from IBlobSite

        public Vector3 GetConnectionPointInDirection(ManhattanDirection direction) {
            switch(direction) {
                case ManhattanDirection.North: return NorthTubeConnectionPoint;
                case ManhattanDirection.South: return SouthTubeConnectionPoint;
                case ManhattanDirection.East:  return EastTubeConnectionPoint;
                case ManhattanDirection.West:  return WestTubeConnectionPoint;
                default: return NorthTubeConnectionPoint;
            }
        }

        public bool CanPlaceBlobInto(ResourceBlob blob) {
            return BlobsWithReservedPositions.Contents.Contains(blob) || CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        protected bool CanPlaceBlobInto_Internal(ResourceBlob blob) {
            return BlobsWithReservedPositions.Contents.Contains(blob) || CanPlaceBlobOfTypeInto_Internal(blob.BlobType);
        }

        public bool CanPlaceBlobOfTypeInto(ResourceType type) {
            return AcceptsPlacement && CanPlaceBlobOfTypeInto_Internal(type);
        }

        protected bool CanPlaceBlobOfTypeInto_Internal(ResourceType type) {
            bool canPlaceInto = BlobsWithin.CanPlaceBlobOfTypeInto(type);
            int spaceLeftWithin = BlobsWithin.GetSpaceLeftForBlobOfType(type);
            int blobsOfTypeReserved = BlobsWithReservedPositions.GetAllBlobsOfType(type).Count();

            bool reservedSpaceLeft = spaceLeftWithin > blobsOfTypeReserved;

            return canPlaceInto && reservedSpaceLeft;
        }

        public void PlaceBlobInto(ResourceBlob blob) {
            if(CanPlaceBlobInto(blob)) {
                PlaceBlobInto_Internal(blob);
            }else {
                throw new BlobException("Cannot place this blob into this BlobSite");
            }
        }

        protected void PlaceBlobInto_Internal(ResourceBlob blob) {
            if(CanPlaceBlobInto_Internal(blob)) {
                BlobsWithReservedPositions.TryExtractBlobFrom(blob);
                BlobsWithin.PlaceBlobInto(blob);
                blob.transform.SetParent(this.transform, true);
                OnBlobPlacedInto(blob);
                RaiseBlobPlacedInto(blob);
            }else {
                throw new BlobException("Cannot internally place this blob into this BlobSite");
            }
        }

        public void ReservePlaceForBlob(ResourceBlob blob) {
            if(CanPlaceBlobInto(blob)) {
                BlobsWithReservedPositions.PlaceBlobInto(blob);
            }else {
                throw new BlobException("Cannot reserve a place for this blob in this BlobSite");
            }
        }

        public void UnreservePlaceForBlob(ResourceBlob blob) {
            BlobsWithReservedPositions.TryExtractBlobFrom(blob);
        }

        public bool CanExtractAnyBlob() {
            return AcceptsExtraction && BlobsWithin.CanExtractAnyBlob();
        }

        public bool CanExtractBlobOfType(ResourceType type) {
            return AcceptsExtraction && BlobsWithin.CanExtractBlobOfType(type);
        }

        public ResourceBlob ExtractAnyBlob() {
            if(CanExtractAnyBlob()) {
                var blobExtracted = BlobsWithin.ExtractAnyBlob();
                DoOnBlobBeingExtracted(blobExtracted);
                RaiseBlobExtractedFrom(blobExtracted);
                return blobExtracted;
            }else {
                throw new BlobException("Cannot extract any blob from this BlobSite");
            }
        }

        public ResourceBlob ExtractBlobOfType(ResourceType type) {
            if(CanExtractBlobOfType(type)) {
                var blobExtracted = BlobsWithin.ExtractBlobOfType(type);
                DoOnBlobBeingExtracted(blobExtracted);
                RaiseBlobExtractedFrom(blobExtracted);
                return blobExtracted;
            }else {
                throw new BlobException("Cannot extract a blob of this type from this BlobSite");
            }
        }

        public IEnumerable<ResourceType> GetExtractableTypes() {
            return BlobsWithin.GetAllTypesWithin();
        }

        public void ClearAllBlobs(bool includeReserved, bool destroyBlobsWithin) {
            var blobList = new List<ResourceBlob>(BlobsWithin.Contents);
            BlobsWithin.Clear();
            if(includeReserved) {
                blobList.AddRange(BlobsWithReservedPositions.Contents);
                BlobsWithReservedPositions.Clear();
            }
            if(destroyBlobsWithin) {
                foreach(var blob in blobList) {
                    Destroy(blob.gameObject);
                }
            }
            RaiseAllBlobsCleared();
        }

        #endregion

        protected virtual void DoOnBlobBeingExtracted(ResourceBlob blobExtracted) { }
        protected virtual void OnBlobPlacedInto(ResourceBlob blobPlaced) { }

        #endregion

    }

}
