using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.Highways {

    [Serializable]
    public class ResourcePermissionEventArgs : EventArgs {

        #region instance fields and properties

        public readonly ResourceType TypeChanged;
        public readonly bool IsNowPermitted;

        #endregion

        #region constructors

        public ResourcePermissionEventArgs(ResourceType typeChanged, bool isNowPermitted) {
            TypeChanged = typeChanged;
            IsNowPermitted = isNowPermitted;
        }

        #endregion

    }

}
