using System;
using System.Runtime.Serialization;

namespace Assets.Map {

    [Serializable]
    public class MapNodeException : Exception {

        #region constructors

        public MapNodeException() {
        }

        public MapNodeException(string message) : base(message) {
        }

        public MapNodeException(string message, Exception innerException) : base(message, innerException) {
        }

        protected MapNodeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        #endregion

    }

}