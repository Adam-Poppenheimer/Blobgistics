using System;
using System.Runtime.Serialization;

namespace Assets.Highways {

    /// <summary>
    /// An exception for errors involving blob highways that aren't covered
    /// by the standard exceptions.
    /// </summary>
    [Serializable]
    public class BlobHighwayException : Exception {

        #region constructors

        /// <inheritdoc/>
        public BlobHighwayException() {
        }

        /// <inheritdoc/>
        public BlobHighwayException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public BlobHighwayException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected BlobHighwayException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}