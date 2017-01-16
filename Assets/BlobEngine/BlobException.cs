using System;
using System.Runtime.Serialization;

namespace Assets.BlobEngine {
    [Serializable]
    internal class BlobException : Exception {
        public BlobException() {
        }

        public BlobException(string message) : base(message) {
        }

        public BlobException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BlobException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}