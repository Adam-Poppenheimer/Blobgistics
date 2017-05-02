using System;
using System.Runtime.Serialization;

namespace Assets.ConstructionZones {

    [Serializable]
    public class ConstructionZoneException : Exception {

        #region constructors

        public ConstructionZoneException() {
        }

        public ConstructionZoneException(string message) : base(message) {
        }

        public ConstructionZoneException(string message, Exception innerException) : base(message, innerException) {
        }

        protected ConstructionZoneException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}