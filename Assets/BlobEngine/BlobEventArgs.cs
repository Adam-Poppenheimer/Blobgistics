using System;

namespace Assets.BlobEngine {

    [Serializable]
    public class BlobEventArgs : EventArgs{

        #region instance fields and properties

        public readonly ResourceBlob Blob;

        #endregion

        #region constructors

        public BlobEventArgs(ResourceBlob blob) {
            Blob = blob;
        }

        #endregion

    }

}