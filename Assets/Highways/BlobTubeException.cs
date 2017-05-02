using System;
using System.Runtime.Serialization;

namespace Assets.Highways {

    [Serializable]
    public class BlobTubeException : Exception {

        public BlobTubeException() {}
        public BlobTubeException(string message) : base(message) {}
        public BlobTubeException(string message, Exception innerException) : base(message, innerException) {}

        protected BlobTubeException(SerializationInfo info, StreamingContext context) : base(info, context) {}

    }

}