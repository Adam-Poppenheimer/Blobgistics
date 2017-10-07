using System;
using System.Runtime.Serialization;

namespace Assets.ConstructionZones {

    /// <summary>
    /// An exception for communicating errors involving construction zones that aren't
    /// covered by the standard exceptions.
    /// </summary>
    [Serializable]
    public class ConstructionZoneException : Exception {

        #region constructors

        /// <inheritdoc/>
        public ConstructionZoneException() {
        }

        /// <inheritdoc/>
        public ConstructionZoneException(string message) : base(message) {
        }

        /// <inheritdoc/>
        public ConstructionZoneException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <inheritdoc/>
        protected ConstructionZoneException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}