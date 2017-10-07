using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.Highways {

    /// <summary>
    /// An EventArgs class for passing data about upkeep request changes.
    /// </summary>
    [Serializable]
    public class UpkeepRequestEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The ResourceType that was changed.
        /// </summary>
        public readonly ResourceType TypeChanged;

        /// <summary>
        /// Whether it is now being requested.
        /// </summary>
        public readonly bool IsBeingRequested;

        #endregion

        #region constructors

        /// <summary>
        /// Creates an event args object reflecting a particular permission change.
        /// </summary>
        /// <param name="typeChanged">The ResourceType whose upkeep request was just changed</param>
        /// <param name="isBeingRequested">Whether it is now being requested</param>
        public UpkeepRequestEventArgs(ResourceType typeChanged, bool isBeingRequested) {
            TypeChanged = typeChanged;
            IsBeingRequested = isBeingRequested;
        }

        #endregion

    }

}
