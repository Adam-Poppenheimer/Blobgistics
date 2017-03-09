using System;
using System.Runtime.Serialization;

namespace Assets.Highways {

    [Serializable]
    public class BlobHighwayException : Exception {

        #region constructors

        public BlobHighwayException() {
        }

        public BlobHighwayException(string message) : base(message) {
        }

        public BlobHighwayException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BlobHighwayException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}