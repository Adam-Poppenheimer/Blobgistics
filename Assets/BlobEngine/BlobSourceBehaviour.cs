using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityCustomUtilities.Extensions;

using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class BlobSourceBehaviour : MonoBehaviour, IBlobSource {

        #region instance fields and properties

        #region from ITubableObject

        public abstract Vector3 EastTubeConnectionPoint  { get; }
        public abstract Vector3 NorthTubeConnectionPoint { get; }
        public abstract Vector3 SouthTubeConnectionPoint { get; }
        public abstract Vector3 WestTubeConnectionPoint  { get; }

        #endregion

        protected abstract BlobPileCapacity Capacity { get; }

        protected BlobPile BlobsWithin;

        #endregion

        #region events

        #region from IBlobSource

        public event EventHandler<BlobEventArgs> NewBlobAvailable;

        protected void RaiseNewBlobAvailable(ResourceBlob newBlob) {
            if(NewBlobAvailable != null) {
                NewBlobAvailable(this, new BlobEventArgs(newBlob));
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            BlobsWithin = new BlobPile(Capacity);
            DoOnAwake();
        }

        protected virtual void DoOnAwake() { }

        #endregion

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

        #region from IBlobSource

        public bool CanExtractAnyBlob() {
            return BlobsWithin.LastBlobInserted != null && BlobsWithin.CanExtractBlob(BlobsWithin.LastBlobInserted);
        }

        public bool CanExtractBlobOfType(ResourceType type) {
            return BlobsWithin.CanExtractBlobOfType(type);
        }

        public ResourceBlob ExtractAnyBlob() {
            if(CanExtractAnyBlob()) {
                var blobToExtract = BlobsWithin.LastBlobInserted;
                BlobsWithin.ExtractBlob(blobToExtract);
                DoOnBlobBeingExtracted(blobToExtract);
                return blobToExtract;
            }else {
                throw new NotImplementedException("Cannot extract any blob from this BlobSource");
            }
        }

        public ResourceBlob ExtractBlobOfType(ResourceType type) {
            if(CanExtractBlobOfType(type)) {
                var blobToExtract = BlobsWithin.ExtractBlobOfType(type);
                DoOnBlobBeingExtracted(blobToExtract);
                return blobToExtract;
            }else {
                throw new BlobException("Cannot extract a blob of this type from this BlobSource");
            }
        }

        public ResourceType GetTypeOfNextExtractedBlob() {
            if(BlobsWithin.LastBlobInserted != null) {
                return BlobsWithin.LastBlobInserted.BlobType;
            }else {
                throw new BlobException("There is no next blob to extract");
            }
        }

        #endregion

        protected virtual void DoOnBlobBeingExtracted(ResourceBlob blobExtracted) { }

        #endregion

    }

}
