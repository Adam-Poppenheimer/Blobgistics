using System;
using System.Runtime.Serialization;

namespace Assets.Session {

    /// <summary>
    /// An exception class for errors relating to SerializableSessions that are
    /// not covered by the standard exceptions.
    /// </summary>
    [Serializable]
    public class SessionException : Exception {

        #region constructors

        /// <inheritdoc/>
        public SessionException() { }

        /// <inheritdoc/>
        public SessionException(string message) : base(message) { }

        /// <inheritdoc/>
        public SessionException(string message, Exception innerException) : base(message, innerException) { }

        /// <inheritdoc/>
        protected SessionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

    }

}