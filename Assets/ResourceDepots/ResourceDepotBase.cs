using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.ResourceDepots {

    /// <summary>
    /// The abstract base class for resource depots, a gameplay element that facilitates the
    /// transfer and storage of resources.
    /// </summary>
    public abstract class ResourceDepotBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// A ResourceDepotBase-unique ID for this object.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// The map node on which this depot is located.
        /// </summary>
        public abstract MapNodeBase Location { get; }

        /// <summary>
        /// Configuration data that defines this depot's capacity.
        /// </summary>
        public abstract ResourceDepotProfile Profile { get; set; }

        #endregion

        #region instance methods

        /// <summary>
        /// Clears the depot of all contents.
        /// </summary>
        public abstract void Clear();

        #endregion

    }

}
