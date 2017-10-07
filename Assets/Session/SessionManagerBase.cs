using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    /// <summary>
    /// The abstract base class for serialization manager, which handles the conversion
    /// of the main game's current state into a serializable format at either play time
    /// or design time.
    /// </summary>
    public abstract class SessionManagerBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The current session that will be read from or written to.
        /// </summary>
        public abstract SerializableSession CurrentSession { get; set; }

        #endregion

        #region instance methods

        /// <summary>
        /// Takes whatever information is in the runtime and pushes it into the
        /// current session, overriding any data that was already there.
        /// </summary>
        public abstract void PushRuntimeIntoCurrentSession();

        /// <summary>
        /// Takes whatever information is in the current session and pushes it into the
        /// runtime, overriding everything that was already there.
        /// </summary>
        public abstract void PullRuntimeFromCurrentSession();

        #endregion

    }

}
