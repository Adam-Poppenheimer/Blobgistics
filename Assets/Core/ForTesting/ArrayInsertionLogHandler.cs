using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class ListInsertionLogHandler : ILogHandler {

        #region instance fields and properties

        public ReadOnlyCollection<KeyValuePair<Exception, UnityEngine.Object>> StoredExceptions {
            get { return storedExceptions.AsReadOnly(); }
        }
        private List<KeyValuePair<Exception, UnityEngine.Object>> storedExceptions = new List<KeyValuePair<Exception, UnityEngine.Object>>();

        public ReadOnlyCollection<DebugMessageData> StoredMessages {
            get { return storedMessages.AsReadOnly(); }
        }
        private List<DebugMessageData> storedMessages = new List<DebugMessageData>();

        #endregion

        #region instance methods

        #region from ILogHandler

        public void LogException(Exception exception, UnityEngine.Object context) {
            storedExceptions.Add(new KeyValuePair<Exception, UnityEngine.Object>(exception, context));
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) {
            storedMessages.Add(new DebugMessageData(logType, context, String.Format(format, args)));
        }

        #endregion

        #endregion
        
    }

}
