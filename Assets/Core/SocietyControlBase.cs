using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

namespace Assets.Core {

    /// <summary>
    /// An abstract base class designed to act as a facade by which the UI can
    /// access parts of the simulation related to societies.
    /// </summary>
    public abstract class SocietyControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Sets whether or not the society with the given ID is permitted to ascend at all.
        /// </summary>
        /// <param name="societyID">The ID of the society to be modified</param>
        /// <param name="ascensionPermitted">Whether or not to permit ascension of any sort</param>
        public abstract void SetGeneralAscensionPermissionForSociety(int societyID, bool ascensionPermitted);

        /// <summary>
        /// Sets whether or not the society with the given ID is permitted to ascend into
        /// the given complexity.
        /// </summary>
        /// <param name="societyID">The ID of the society to be modified</param>
        /// <param name="complexity">The complexity whose ascension permission is in question</param>
        /// <param name="ascensionPermitted">Whether the society will be allowed to ascend to the given complexity</param>
        public abstract void SetSpecificAscensionPermissionForSociety(int societyID, ComplexityDefinitionBase complexity, bool ascensionPermitted);

        /// <summary>
        /// Destroys the society with the given ID, if it exists.
        /// </summary>
        /// <param name="societyID">The ID of the society to destroy</param>
        public abstract void DestroySociety(int societyID);

        #endregion

    }

}
