using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    /// <summary>
    /// An abstract base class that defines a set of algorithms MapGraph needs in order to function correctly.
    /// </summary>
    /// <remarks>
    /// These algorithms are separated into a different class in order to keep MapGraph clean,
    /// to make re-use easier, and to hedge against the possibility of multiple implementations.
    /// It also encourages the implementation of stateless algorithms, which makes testing easier.
    /// </remarks>
    public abstract class MapGraphAlgorithmSetBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Gets the distance, in edges, between a pair of given nodes if a connection exists between them.
        /// </summary>
        /// <remarks>
        /// Unlike in MapGraph, this method considers only the neighbor relations defined on the various nodes.
        /// </remarks>
        /// <param name="node1">The first node to consider</param>
        /// <param name="node2">The second node to consider</param>
        /// <param name="allNodes">A record of all nodes to be considered for the path</param>
        /// <returns>The distance, in edges, between the two endpoints, or int.MaxValue if no path exists</returns>
        public abstract int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2, IEnumerable<MapNodeBase> allNodes);

        /// <summary>
        /// Gets the shortest path between the starting and ending nodes, traversing only the nodes specified
        /// in allNodes. If a path exists, returns a list whose first and last elements are the starting and ending points,
        /// with a number of intervening nodes that represent a valid path. Otherwise, the list is just the starting node.
        /// </summary>
        /// <param name="start">The node to begin traversal from</param>
        /// <param name="end">The desired node to reach</param>
        /// <param name="allNodes">All nodes to consider for the path ahead</param>
        /// <returns>A list containing the path from start to end (capped by those two values) if a path exists.
        /// Otherwise, a list containing only start</returns>
        public abstract List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase start, MapNodeBase end, IEnumerable<MapNodeBase> allNodes);

        /// <summary>
        /// Gets the nearest node to the endpoints of a given edge of origin that satisfies a specified condition.
        /// Will only return nodes within a specified maximum distance, and reports the shortest distance from one
        /// of the two endpoints without specifying which endpoint is closer.
        /// </summary>
        /// <remarks>
        /// Note that this method extracts neighbors directly from the MapNodes rather than routing through MapGraph.
        /// </remarks>
        /// <param name="edgeOfOrigin">The edge whose endpoints are the centers of the search</param>
        /// <param name="condition">The condition that the nearest node must satisfy</param>
        /// <param name="maxDistance">The maximum distance the method will search to find a node meeting the conditions</param>
        /// <returns>
        /// A NodeDistanceSummary containing the nearest valid node and its distance from one of the
        /// endpoints of the edgeOfOrigin, or null and some arbitrary number of none was found.
        /// </returns>
        public abstract NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin, Predicate<MapNodeBase> condition, int maxDistance);

        /// <summary>
        /// Gets the nearest node to the given node that satisfies a specified condition.
        /// Will only return nodes within a specified maximum distance, and reports the 
        /// shortest distance from the node.
        /// </summary>
        /// <param name="rootNode">The node that centers the search</param>
        /// <param name="condition">The condition that the nearest node must satisfy</param>
        /// <param name="maxDistance">The maximum distance the method will search to find a node meeting the conditions</param>
        /// <returns>
        /// A NodeDistanceSummary containing the nearest valid node and its distance from
        /// the root node, or null and some arbitrary number of none was found.
        /// </returns>
        public abstract NodeDistanceSummary GetNearestNodeToNodeWhere(MapNodeBase rootNode, Predicate<MapNodeBase> condition, int maxDistance);

        #endregion

    }

}
