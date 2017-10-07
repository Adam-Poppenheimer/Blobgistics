
using System;

namespace Assets.Session {

    /// <summary>
    /// An EventArgs class for events involving SerializableSessions.
    /// </summary>
    [Serializable]
    public class SerializableSessionEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The session that caused the event.
        /// </summary>
        public readonly SerializableSession Session;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new event args with the given session.
        /// </summary>
        /// <param name="session">The session that caused the event.</param>
        public SerializableSessionEventArgs(SerializableSession session) {
            Session = session;
        }

        #endregion

    }

}