using System;
using System.Runtime.Serialization;

namespace Assets.Depots {

    [Serializable]
    public class ResourceDepotException : Exception {

        #region constructors

        public ResourceDepotException() {
        }

        public ResourceDepotException(string message) : base(message) {
        }

        public ResourceDepotException(string message, Exception innerException) : base(message, innerException) {
        }

        protected ResourceDepotException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}