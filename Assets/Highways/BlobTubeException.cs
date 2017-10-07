using System;
using System.Runtime.Serialization;

namespace Assets.Highways {

    /// <summary>
    /// An exception class for errors related to blob tubes that aren't
    /// covered by the standard exceptions.
    /// </summary>
    [Serializable]
    public class BlobTubeException : Exception {

        /// <inheritdoc/>
        public BlobTubeException() {}

        /// <inheritdoc/>
        public BlobTubeException(string message) : base(message) {}

        /// <inheritdoc/>
        public BlobTubeException(string message, Exception innerException) : base(message, innerException) {}

        /// <inheritdoc/>
        protected BlobTubeException(SerializationInfo info, StreamingContext context) : base(info, context) {}

    }

}