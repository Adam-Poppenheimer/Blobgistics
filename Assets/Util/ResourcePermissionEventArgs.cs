using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.Util {

    /// <summary>
    /// An EventArgs class for passing data about resource permission changes.
    /// </summary>
    [Serializable]
    public class ResourcePermissionEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The ResourceType that was changed.
        /// </summary>
        public readonly ResourceType TypeChanged;

        /// <summary>
        /// Whether it is now permitted.
        /// </summary>
        public readonly bool IsNowPermitted;

        #endregion

        #region constructors

        /// <summary>
        /// Creates an event args object reflecting a particular permission change.
        /// </summary>
        /// <param name="typeChanged">The ResourceType whose permission was just changed</param>
        /// <param name="isNowPermitted">Whether it is now permitted</param>
        public ResourcePermissionEventArgs(ResourceType typeChanged, bool isNowPermitted) {
            TypeChanged = typeChanged;
            IsNowPermitted = isNowPermitted;
        }

        #endregion

    }

}
