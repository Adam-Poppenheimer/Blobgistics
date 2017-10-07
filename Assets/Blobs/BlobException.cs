using System;
using System.Runtime.Serialization;

namespace Assets.Blobs {

    /// <summary>
    /// An exception for errors that arise when processing blobs and don't fit into standard
    /// exception types.
    /// </summary>
    [Serializable]
    internal class BlobException : Exception {

        #region constructors

        /// <inheritdoc/>
        public BlobException() {
        }

        /// <inheritdoc/>
        public BlobException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public BlobException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected BlobException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}