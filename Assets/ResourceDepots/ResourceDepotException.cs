using System;
using System.Runtime.Serialization;

namespace Assets.ResourceDepots {

    /// <summary>
    /// An exception class for errors involving ResourceDepots that are not
    /// covered by the standard exceptions.
    /// </summary>
    [Serializable]
    public class ResourceDepotException : Exception {

        #region constructors

        /// <inheritdoc/>
        public ResourceDepotException() {
        }

        /// <inheritdoc/>
        public ResourceDepotException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public ResourceDepotException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected ResourceDepotException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}