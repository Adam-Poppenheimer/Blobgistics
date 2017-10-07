using System;
using System.Runtime.Serialization;

namespace Assets.HighwayManager {

    /// <summary>
    /// An exception class to handle errors relating to highway managers that aren't
    /// covered by the standard exceptions.
    /// </summary>
    [Serializable]
    public class HighwayManagerException : Exception {

        #region constructors

        /// <inheritdoc/>
        public HighwayManagerException() {
        }

        /// <inheritdoc/>
        public HighwayManagerException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public HighwayManagerException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected HighwayManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}