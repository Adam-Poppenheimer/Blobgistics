using System;
using System.Runtime.Serialization;

namespace Assets.BlobSites {

    [Serializable]
    public class BlobSiteException : Exception {

        #region constructors

        public BlobSiteException() {
        }

        public BlobSiteException(string message) : base(message) {
        }

        public BlobSiteException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BlobSiteException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}