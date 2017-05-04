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

        public List<KeyValuePair<Exception, UnityEngine.Object>> StoredExceptions = new List<KeyValuePair<Exception, UnityEngine.Object>>();

        public List<DebugMessageData> StoredMessages = new List<DebugMessageData>();

        #endregion

        #region instance methods

        #region from ILogHandler

        public void LogException(Exception exception, UnityEngine.Object context) {
            StoredExceptions.Add(new KeyValuePair<Exception, UnityEngine.Object>(exception, context));
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) {
            StoredMessages.Add(new DebugMessageData(logType, context, String.Format(format, args)));
        }

        #endregion

        #endregion
        
    }

}
