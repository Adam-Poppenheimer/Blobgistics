using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Societies {

    /// <summary>
    /// An EventArgs class for complexity ascent permission changes, used
    /// primarily by the UI.
    /// </summary>
    [Serializable]
    public class ComplexityAscentPermissionEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The complexity whose ascension permission just changed.
        /// </summary>
        public readonly ComplexityDefinitionBase Complexity;

        /// <summary>
        /// Its new ascension permission.
        /// </summary>
        public readonly bool AscensionIsPermitted;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new event args with the given parameters.
        /// </summary>
        /// <param name="complexity">The complexity whose ascension permission just changed</param>
        /// <param name="ascensionIsPermitted">Its new ascension permission</param>
        public ComplexityAscentPermissionEventArgs(ComplexityDefinitionBase complexity, bool ascensionIsPermitted) {
            Complexity = complexity;
            AscensionIsPermitted = ascensionIsPermitted;
        }

        #endregion

    }

}
