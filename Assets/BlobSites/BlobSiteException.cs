using System;
using System.Runtime.Serialization;

namespace Assets.BlobSites {

    /// <summary>
    /// The exception class for errors related to blob sites that don't fit under
    /// the standard exceptions.
    /// </summary>
    [Serializable]
    public class BlobSiteException : Exception {

        #region constructors

        /// <inheritdoc/>
        public BlobSiteException() {
        }

        /// <inheritdoc/>
        public BlobSiteException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public BlobSiteException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected BlobSiteException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}