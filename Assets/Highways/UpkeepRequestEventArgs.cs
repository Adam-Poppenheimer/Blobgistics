using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.Highways {

    [Serializable]
    public class UpkeepRequestEventArgs : EventArgs {

        #region instance fields and properties

        public readonly ResourceType Type;
        public readonly bool IsBeingRequested;

        #endregion

        #region constructors

        public UpkeepRequestEventArgs(ResourceType type, bool isBeingRequested) {
            Type = type;
            IsBeingRequested = isBeingRequested;
        }

        #endregion

    }

}
