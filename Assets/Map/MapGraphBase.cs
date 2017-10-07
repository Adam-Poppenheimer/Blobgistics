using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    /// <summary>
    /// An abstract base class that manages the in-game map. Maps are represented as undirected graphs.
    /// Acts as the factory, adjacency canon, and pathfinder for all nodes and edges.
    /// </summary>
    [ExecuteInEditMode]
    public abstract class MapGraphBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// Contains all of the nodes subscribed to the graph.
        /// </summary>
        public abstract ReadOnlyCollection<MapNodeBase> Nodes { get; }
        /// <summary>
        /// Contains all of the edges subscribed to the graph.
        /// </summary>
        public abstract ReadOnlyCollection<MapEdgeBase> Edges { get; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever a MapNodeBase is unsubscribed from this MapGraphBase.
        /// </summary>
        public event EventHandler<MapNodeEventArgs> MapNodeUnsubscribed;

        /// <summary>
        /// Fires whenever a MapEdgeBase is unsubscribed from this MapGraphBase.
        /// </summary>
        public event EventHandler<MapEdgeEventArgs> MapEdgeUnsubscribed;

        /// <summary>
        /// Fires the MapNodeUnsubscribed event.
        /// </summary>
        /// <param name="node">The node to fire on</param>
        protected void RaiseMapNodeUnsubscribed(MapNodeBase node) {
            if(MapNodeUnsubscribed != null) {
                MapNodeUnsubscribed(this, new MapNodeEventArgs(node));
            }
        }

        /// <summary>
        /// Fires the MapEdgeUnsubscribed event.
        /// </summary>
        /// <param name="edge">The edge to fire on</param>
        protected void RaiseMapEdgeUnsubscribed(MapEdgeBase edge) {
            if(MapEdgeUnsubscribed != null) {
                MapEdgeUnsubscribed(this, new MapEdgeEventArgs(edge));
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Constructs a new node object at a given position, subscribes it, and returns it.
        /// </summary>
        /// <param name="localPosition">The position relative to this map graph where the new node will be located</param>
        /// <returns></returns>
        public abstract MapNodeBase BuildNode(Vector3 localPosition);

        /// <summary>
        /// As BuildNode(Vector3), but also sets the node with some starting terrain.
        /// </summary>
        /// <param name="localPosition"></param>
        /// <param name="startingTerrain"></param>
        /// <returns>The node just created with the specified local position and terrain</returns>
        public abstract MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain);

        /// <summary>
        /// Unsubscribes and destroys the specified node.
        /// </summary>
        /// <param name="node">The node to destroy</param>
        public abstract void DestroyNode(MapNodeBase node);

        /// <summary>
        /// Subscribes an existing node to the graph. This will include it in the Nodes property
        /// and make it a child to this MapGraph.
        /// </summary>
        /// <param name="node">The node to subscribe</param>
        public abstract void SubscribeNode  (MapNodeBase node);

        /// <summary>
        /// Unsubscribes an existing node to the graph. Unsubscribed nodes are not considered
        /// in pathfinding and adjacency operations, and do not appear in the Nodes property.
        /// See also <seealso cref="SubscribeNode(MapNodeBase)"/>
        /// </summary>
        /// <param name="node">The node to unsubscribe</param>
        public abstract void UnsubscribeNode(MapNodeBase node);

        /// <summary>
        /// Constructs a MapEdgeBase object whose endpoints are the specified arguments.
        /// This operation must also subscribe the MapEdge.
        /// </summary>
        /// <remarks>
        /// Note that unsubscribing a node also unsubscribes any edges that referenced it.
        /// </remarks>
        /// <param name="firstEndpoint">The first endpoint of the resulting edge</param>
        /// <param name="secondEndpoint">The second endpoint of the resulting edge</param>
        /// <returns>The edge created, which has been subscribed to the MapGraphBase</returns>
        public abstract MapEdgeBase BuildMapEdge(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        /// <summary>
        /// Destroys some map edge lying between the two nodes if one exists, or does nothing
        /// if the edge does not exist. The order of the endpoints does not matter.
        /// Calls into <seealso cref="DestroyMapEdge(MapEdgeBase)"/>
        /// </summary>
        /// <param name="first">One of the endpoints for the hypothetical edge</param>
        /// <param name="second">The other endpoint for the hypothetical edge</param>
        public abstract void DestroyMapEdge(MapNodeBase first, MapNodeBase second);

        /// <exception>ArgumentNullException if edge is null</exception>
        /// <param name="edge">The edge to destroy</param>
        public abstract void DestroyMapEdge(MapEdgeBase edge);

        /// <summary>
        /// Subscribes an existing edge to the MapGraphBase. Similar to <seealso cref="SubscribeNode(MapNodeBase)"/>.
        /// </summary>
        /// <param name="edge">The edge to subscribe</param>
        public abstract void SubscribeMapEdge(MapEdgeBase edge);
        /// <summary>
        /// Unsubscribes an existing edge from MapGraphBase. Unsubscribed edges are not considered
        /// in pathfinding and adjacency operations, and do not appear in the Edges property.
        /// </summary>
        /// <param name="edge">The edge to unusbscribe</param>
        public abstract void UnsubscribeMapEdge(MapEdgeBase edge);

        /// <summary>
        /// Retrieves a node in the Nodes property with a given ID, if one exists.
        /// </summary>
        /// <param name="id">The node of the given ID, or null if non is found</param>
        /// <returns></returns>
        public abstract MapNodeBase GetNodeOfID(int id);

        /// <summary>
        /// Retrives an edge in the Edges property that contains both the specified endpoints,
        /// or null if none exists. The order of the endpoints doesn't matter.
        /// </summary>
        /// <param name="endpointOne">One of the endpoints of the hypothetical edge</param>
        /// <param name="endpointTwo">The other endpoint of the hypothetical edge</param>
        /// <returns></returns>
        public abstract MapEdgeBase GetEdge(MapNodeBase endpointOne, MapNodeBase endpointTwo);

        /// <summary>
        /// Retrieves all other nodes connected directly to this one by some edge in Edges.
        /// </summary>
        /// <param name="node">The node to consider</param>
        /// <returns>All of the nodes neighboring the specified node</returns>
        public abstract IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node);
        /// <summary>
        /// Retrieves all edges that contain the specified node as the endpoint.
        /// </summary>
        /// <param name="node">The node to consider</param>
        /// <returns>All edges that specify the node as an endpoint.</returns>
        public abstract IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node);

        /// <summary>
        /// Returns the shortest distance, in edges, between two nodes. If there exists no path
        /// between the nodes, or int.MaxValue if no path exists.
        /// </summary>
        /// <param name="nodeOne"></param>
        /// <param name="nodeTwo"></param>
        /// <returns>The distance in edges between the two nodes, or int.MaxValue if no path exists</returns>
        public abstract int GetDistanceBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo);

        /// <summary>
        /// Returns a list containing the shortest path between the given nodes. This list always contains the starting
        /// element as its first entry. If there exists a path, the last element in the list is the terminating node.
        /// If there is no path, the rest of the list is empty.
        /// </summary>
        /// <param name="nodeOne">The starting point, which will always appear at the start of the returned list</param>
        /// <param name="nodeTwo">The terminating point</param>
        /// <returns>A list of edge-connected nodes, starting with nodeOne, that represents the desired path,
        /// or contains only nodeOne if no path exists</returns>
        public abstract List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo);

        /// <summary>
        /// Finds the closest node to some specified edge that satisfied the specified conditions within
        /// some optional maximum distance. Distance is considered independently from each of the edge's
        /// endpoints and will return the smaller value between the two.
        /// </summary>
        /// <param name="edgeOfOrigin">The edge that defines the starting nodes of our search</param>
        /// <param name="condition">The condition that a given node must satisfy</param>
        /// <param name="maxDistance">The maximum distance away from the edgeOfOrigin's endpoint that the 
        /// method will check for</param>
        /// <returns>A record of which node was encountered and how far away it was</returns>
        public abstract NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin,
            Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue);

        #endregion

    }

}
