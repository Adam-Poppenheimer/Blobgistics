using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    /// <summary>
    /// Used to return results on some MapGraphBase and MapGraphAlgorithmSetBase methods.
    /// </summary>
    public class NodeDistanceSummary {

        #region instance fields and properties

        /// <summary>
        /// The node that was found.
        /// </summary>
        public readonly MapNodeBase Node;

        /// <summary>
        /// The distance of the discovered node from the starting element.
        /// </summary>
        public readonly int Distance;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a NodeDistanceSummary with the given node and distance.
        /// </summary>
        /// <param name="node">The node that was found</param>
        /// <param name="distance">The distance of the discovered node from the starting element</param>
        public NodeDistanceSummary(MapNodeBase node, int distance) {
            Node = node;
            Distance = distance;
        }

        #endregion

    }

}
