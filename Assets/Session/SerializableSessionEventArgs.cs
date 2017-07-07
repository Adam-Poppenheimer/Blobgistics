
using System;

namespace Assets.Session {

    [Serializable]
    public class SerializableSessionEventArgs : EventArgs {

        #region instance fields and properties

        public readonly SerializableSession Session;

        #endregion

        #region constructors

        public SerializableSessionEventArgs(SerializableSession session) {
            Session = session;
        }

        #endregion

    }

}