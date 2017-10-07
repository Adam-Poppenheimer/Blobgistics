using System;
using System.Runtime.Serialization;

namespace Assets.Societies {

    /// <summary>
    /// An exception class for handling errors related to Society that are
    /// not covered by the standard exceptions.
    /// </summary>
    [Serializable]
    public class SocietyException : Exception {

        #region constructors

        /// <inheritdoc/>
        public SocietyException() {
        }

        /// <inheritdoc/>
        public SocietyException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public SocietyException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected SocietyException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}