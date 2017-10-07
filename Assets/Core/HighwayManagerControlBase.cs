using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;

namespace Assets.Core {

    /// <summary>
    /// An abstract base class that acts as a facade by which the UI can
    /// access parts of the simulation relating to highway managers.
    /// </summary>
    public abstract class HighwayManagerControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Destroys the highway manager with the given ID, if it exists.
        /// </summary>
        /// <param name="managerID">The ID of the manager to destroy</param>
        public abstract void DestroyHighwayManagerOfID(int managerID);

        /// <summary>
        /// Gets UI summaries for all highways that are managed by the highway manager
        /// with the given ID, if such a manager exists.
        /// </summary>
        /// <param name="managerID">The ID of the manager to consider</param>
        /// <returns></returns>
        public abstract IEnumerable<BlobHighwayUISummary> GetHighwaysManagedByManagerOfID(int managerID);

        #endregion

    }

}
