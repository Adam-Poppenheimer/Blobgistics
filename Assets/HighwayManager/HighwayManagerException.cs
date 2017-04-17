using System;
using System.Runtime.Serialization;

namespace Assets.HighwayManager {

    [Serializable]
    public class HighwayManagerException : Exception {

        #region constructors

        public HighwayManagerException() {
        }

        public HighwayManagerException(string message) : base(message) {
        }

        public HighwayManagerException(string message, Exception innerException) : base(message, innerException) {
        }

        protected HighwayManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}