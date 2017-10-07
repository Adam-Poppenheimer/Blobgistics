using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

namespace Assets.ConstructionZones {

    /// <summary>
    /// The abstract base class for all construction zones, which enable the creation of societies, resource depots,
    /// highway managers, and the addition and removal of forests by the player.
    /// </summary>
    public abstract class ConstructionZoneBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// An ID unique among all ConstructionZoneBases.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// The map node the construction zone is placed upon.
        /// </summary>
        public abstract MapNodeBase Location { get; }

        /// <summary>
        /// The construction project this construction zone is trying to complete.
        /// </summary>
        public abstract ConstructionProjectBase CurrentProject { get; set; }

        /// <summary>
        /// Whether or not the current project has been completed.
        /// </summary>
        public abstract bool ProjectHasBeenCompleted { get; }

        #endregion

    }

}
