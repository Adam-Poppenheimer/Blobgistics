using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    public abstract class SessionManagerBase : MonoBehaviour {

        #region instance fields and properties

        public abstract SerializableSession CurrentSession { get; set; }

        #endregion

        #region instance methods

        public abstract void PushRuntimeIntoCurrentSession();

        public abstract void PullRuntimeFromCurrentSession();

        #endregion

    }

}
