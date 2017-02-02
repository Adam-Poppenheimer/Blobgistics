using System;
using System.Runtime.Serialization;

namespace Assets.Map {

    [Serializable]
    public class MapGraphException : Exception {

        #region constructors

        public MapGraphException() { }

        public MapGraphException(string message) : base(message) { }

        public MapGraphException(string message, Exception innerException) : base(message, innerException) { }

        protected MapGraphException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

    }
}