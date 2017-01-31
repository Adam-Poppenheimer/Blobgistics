using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class BlobSourceAndTargetBehaviour : BlobTargetBehaviour, IBlobSource {

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

        public new void Initialize() {
            BlobsWithin = new BlobPile();
            BlobsWithin.Capacity = BlobPileCapacity.NoCapacity;
            BlobsWithReservedPositions = new BlobPile();
            BlobsWithReservedPositions.Capacity = BlobPileCapacity.NoCapacity;
            BlobInsertedInto += BlobSourceAndTargetBehaviour_BlobInsertedInto;
            DoOnInitialize();
        }

        protected virtual void DoOnBlobBeingExtracted(ResourceBlob blobExtracted) { }

        private void BlobSourceAndTargetBehaviour_BlobInsertedInto(object sender, BlobEventArgs e) {
            RaiseBlobInsertedInto(e.Blob);
        }

        #endregion

    }

}
