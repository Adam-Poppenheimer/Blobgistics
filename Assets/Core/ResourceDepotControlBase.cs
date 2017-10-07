using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Core {

    /// <summary>
    /// An abstract base class designed to act as a facade by which the UI
    /// can access parts of the simulation related to resource depots.
    /// </summary>
    public abstract class ResourceDepotControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Destroys the resource depot with the given ID, if it exists.
        /// </summary>
        /// <param name="depotID">The ID of the resource depot to destroy</param>
        public abstract void DestroyResourceDepotOfID(int depotID);

        #endregion

    }

}
