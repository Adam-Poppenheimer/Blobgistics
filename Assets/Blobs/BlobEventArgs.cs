using System;

namespace Assets.Blobs {

    /// <summary>
    /// An EventArgs class for events that involve ResourceBlobBases.
    /// </summary>
    [Serializable]
    public class BlobEventArgs : EventArgs{

        #region instance fields and properties

        /// <summary>
        /// The blob that triggered the event.
        /// </summary>
        public readonly ResourceBlobBase Blob;

        #endregion

        #region constructors

        /// <summary/>
        public BlobEventArgs(ResourceBlobBase blob) {
            Blob = blob;
        }

        #endregion

    }

}