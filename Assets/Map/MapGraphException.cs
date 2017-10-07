using System;
using System.Runtime.Serialization;

namespace Assets.Map {

    /// <summary>
    /// A general exception for all MapGraph-related errors that aren't
    /// easily covered under one of the standard exceptions.
    /// </summary>
    [Serializable]
    public class MapGraphException : Exception {

        #region constructors

        /// <inheritdoc/>
        public MapGraphException() { }

        /// <inheritdoc/>
        public MapGraphException(string message) : base(message) { }

        /// <inheritdoc/>
        public MapGraphException(string message, Exception innerException) : base(message, innerException) { }

        /// <inheritdoc/>
        protected MapGraphException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

    }
}