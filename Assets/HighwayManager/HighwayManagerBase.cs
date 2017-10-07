using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;
using Assets.Map;
using Assets.Core;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager {

    /// <summary>
    /// The abstract base class for highway managers, gameplay elements that can provide
    /// increased speed and efficiency to highways at the cost of resources.
    /// </summary>
    public abstract class HighwayManagerBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// An ID unique amongst all HighwayManagerBases.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// The location the manager occupies.
        /// </summary>
        public abstract MapNodeBase Location { get; }

        /// <summary>
        /// The last upkeep the manager calculated, informed by the highways that it is managing.
        /// </summary>
        public abstract ReadOnlyDictionary<ResourceType, int> LastCalculatedUpkeep { get; }

        #endregion

        #region instance methods

        /// <summary>
        /// Increments the highway manager simulation by some number of seconds.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds that have passed since the last call to Tick</param>
        public abstract void Tick(float secondsPassed);

        #endregion

    }

}
