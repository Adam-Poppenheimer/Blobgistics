using System;
using System.Runtime.Serialization;

namespace Assets.Societies {
    [Serializable]
    internal class SocietyException : Exception {
        public SocietyException() {
        }

        public SocietyException(string message) : base(message) {
        }

        public SocietyException(string message, Exception innerException) : base(message, innerException) {
        }

        protected SocietyException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}