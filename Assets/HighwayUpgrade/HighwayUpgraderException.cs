using System;
using System.Runtime.Serialization;

namespace Assets.HighwayUpgrade {

    [Serializable]
    public class HighwayUpgraderException : Exception {

        #region constructors

        public HighwayUpgraderException() {
        }

        public HighwayUpgraderException(string message) : base(message) {
        }

        public HighwayUpgraderException(string message, Exception innerException) : base(message, innerException) {
        }

        protected HighwayUpgraderException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}