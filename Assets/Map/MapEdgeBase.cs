using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    /// <summary>
    /// The abstract base class from which all map edges derive. A map edge is a connection between
    /// two map nodes upon which highways can be built. The MapEdgeBase class is provided
    /// primarily for convenience, and a more performant Map paradigm would likely not
    /// include them in their current form.
    /// </summary>
    [SelectionBase]
    public abstract class MapEdgeBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// A unique ID that the UI can use to identify instances without accessing them directly.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// The first endpoint of the edge. Endpoint order is largely irrelevant.
        /// </summary>
        public abstract MapNodeBase FirstNode  { get; }
        /// <summary>
        /// The second endpoint of the edge. Endpoint order is largely irrelevant.
        /// </summary>
        public abstract MapNodeBase SecondNode { get; }

        /// <summary>
        /// The MapGraphBase that's supposed to be managing this edge.
        /// </summary>
        /// <remarks>
        /// Because of complications arising from prefab instantiating and design-time
        /// copying, it's sometimes necessary for MapEdgeBases to be subscribed upon their
        /// creation. Since MapGraphBase has no way of knowing when a new MapEdgeBase
        /// has been created, it's up to the MapEdgeBase itself to subscribe itself.
        /// </remarks>
        public abstract MapGraphBase ParentGraph { get; set; }

        #endregion

        #region instance methods

        #region from Object

        /// <inheritdoc/>
        public override string ToString() {
            return name;
        }

        #endregion

        /// <summary>
        /// Modifies the MapEdgeBase so that it's algined with its endpoints.
        /// </summary>
        public abstract void RefreshOrientation();

        #endregion

    }

}
