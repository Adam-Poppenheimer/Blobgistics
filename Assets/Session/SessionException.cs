using System;
using System.Runtime.Serialization;

namespace Assets.Session {

    [Serializable]
    public class SessionException : Exception {

        #region constructors

        public SessionException() { }
        public SessionException(string message) : base(message) { }
        public SessionException(string message, Exception innerException) : base(message, innerException) { }

        protected SessionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

    }

}