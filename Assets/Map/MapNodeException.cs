using System;
using System.Runtime.Serialization;

namespace Assets.Map {

    /// <summary>
    /// An exception class for errors related to MapNodes that don't fall easily
    /// into more standard categories.
    /// </summary>
    [Serializable]
    public class MapNodeException : Exception {

        #region constructors

        /// <inheritdoc/>
        public MapNodeException() {
        }

        /// <inheritdoc/>
        public MapNodeException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public MapNodeException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected MapNodeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}