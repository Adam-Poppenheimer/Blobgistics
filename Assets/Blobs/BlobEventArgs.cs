using System;

namespace Assets.Blobs {

    [Serializable]
    public class BlobEventArgs : EventArgs{

        #region instance fields and properties

        public readonly ResourceBlobBase Blob;

        #endregion

        #region constructors

        public BlobEventArgs(ResourceBlobBase blob) {
            Blob = blob;
        }

        #endregion

    }

}