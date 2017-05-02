using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Core.ForTesting {

    public class DebugMessageData {

        #region instance fields and properties

        public readonly LogType LogType;
        public readonly UnityEngine.Object Context;
        public readonly string Message;

        #endregion

        #region constructors

        public DebugMessageData(LogType logType, UnityEngine.Object context, string message) {
            LogType = logType;
            Context = context;
            Message = message;
        }

        #endregion

    }

}
