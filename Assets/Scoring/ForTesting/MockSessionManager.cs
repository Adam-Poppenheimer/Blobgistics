using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Session;

namespace Assets.Scoring.ForTesting {

    public class MockSessionManager : SessionManagerBase {

        #region instance fields and properties

        #region from SessionManagerBase

        public override SerializableSession CurrentSession {
            get {
                if(_currentSession == null) {
                    _currentSession = new SerializableSession("Name", "Description", 100);
                }
                return _currentSession;
            }
            set { _currentSession = value; }
        }
        private SerializableSession _currentSession;

        #endregion

        #endregion

        #region instance methods

        #region from SessionManagerBase

        public override void PullRuntimeFromCurrentSession() {
            throw new NotImplementedException();
        }

        public override void PushRuntimeIntoCurrentSession() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
