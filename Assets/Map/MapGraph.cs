using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.BlobSites;
using Assets.Core;
using Assets.Highways;

namespace Assets.Map {

    /// <summary>
    /// The standard implementation for MapGraphBase, which represents the game map as an
    /// undirected graph. Acts as the factory, adjacency canon, and pathfinder for all
    /// nodes and edges.
    /// </summary>
    /// <remarks>
    /// MapGraphs generate new Nodes and Edges through prefabs, configured in the inspector,
    /// that are then instantiated and provided with their dependencies from the runtime.
    /// 
    /// MapGraphs also operate in EditMode, which means that they make use of DestroyImmediate
    /// instead of Destroy and contend with Unity's serializer and transfers between runtime
    /// environments.
    /// </remarks>
    [ExecuteInEditMode]    
    public class MapGraph : MapGraphBase {

        #region instance fields and properties

        #region from MapGraphBase
        /// <inheritdoc/>
        public override ReadOnlyCollection<MapNodeBase> Nodes {
            get { return nodes.AsReadOnly(); }
        }
        private List<MapNodeBase> nodes = new List<MapNodeBase>();

        /// <inheritdoc/>
        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get { return edges.AsReadOnly(); }
        }
        private List<MapEdgeBase> edges = new List<MapEdgeBase>();

        #endregion

        /// <summary>
        /// A dependency that must be injected into all newly subscribed nodes.
        /// </summary>
        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        /// <summary>
        /// A dependency that must be injected into all newly subscribed nodes.
        /// </summary>
        public TerrainMaterialRegistry TerrainMaterialRegistry {
            get { return _terrainMaterialRegistry; }
            set { _terrainMaterialRegistry = value; }
        }
        [SerializeField] private TerrainMaterialRegistry _terrainMaterialRegistry;

        /// <summary>
        /// A dependency that must be injected into all newly subscribed nodes.
        /// </summary>
        public TerrainGrid TerrainTileGrid {
            get { return _terrainTileGrid; }
            set { _terrainTileGrid = value; }
        }
        [SerializeField] private TerrainGrid _terrainTileGrid;

        /// <summary>
        /// An object containing implementations for a number of graph traversal algorithms the
        /// MapGraph will use to determine paths and the nearest nodes and edges with certain
        /// conditions.
        /// </summary>
        public MapGraphAlgorithmSetBase AlgorithmSet {
            get { return _algorithmSet; }
            set { _algorithmSet = value; }
        }
        [SerializeField] private MapGraphAlgorithmSetBase _algorithmSet;

        /// <summary>
        /// A depndency that must be injected into the BlobSites of all newly subscribed nodes.
        /// Is also placed into the now-obsolete BlobSites on all subsribed MapEdges.
        /// </summary>
        public BlobSiteConfigurationBase BlobSiteConfiguration {
            get { return _blobSiteConfiguration; }
            set { _blobSiteConfiguration = value; }
        }
        [SerializeField] private BlobSiteConfigurationBase _blobSiteConfiguration;

        [SerializeField] private GameObject NodePrefab;
        [SerializeField] private GameObject EdgePrefab;

        /// <remarks>
        /// </remarks>
        private DictionaryOfLists<MapNodeBase, MapNodeBase> NeighborsOfNode = 
            new DictionaryOfLists<MapNodeBase, MapNodeBase>();

        #endregion

        #region instance methods

        #region Unity event methods

        //These methods are hedges against possible state inconsistency. The current implementation
        //Requires MapGraph to be subscribed to every Node and Edge in the runtime in order for those
        //nodes and edges to function.

        private void OnDestroy() {
            foreach(var node in new List<MapNodeBase>(nodes)) {
                UnsubscribeNode(node);
            }
            foreach(var edge in new List<MapEdgeBase>(edges)) {
                UnsubscribeMapEdge(edge);
            }
            nodes.Clear();
            edges.Clear();
        }

        private void Start() {
            foreach(var node in GetComponentsInChildren<MapNodeBase>()) {
                SubscribeNode(node);
            }
            foreach(var edge in GetComponentsInChildren<MapEdgeBase>()) {
                SubscribeMapEdge(edge);
            }
        }

        #endregion

        #region from MapGraphBase

        /// <inheritdoc/>
        public override MapNodeBase BuildNode(Vector3 localPosition) {
            return BuildNode(localPosition, TerrainType.Grassland);
        }

        /// <inheritdoc/>
        public override MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain) {
            MapNode newNode = null;
            if(NodePrefab != null) {
                var nodeObject = Instantiate(NodePrefab, this.transform, false) as GameObject;
                newNode = nodeObject.GetComponent<MapNode>();
                if(newNode == null) {
                    throw new MapNodeException("The MapNode prefab lacked a MapNode component");
                }
            }else {
                var hostingObject = new GameObject();
                newNode = hostingObject.AddComponent<MapNode>();
                newNode.SetBlobSite(newNode.gameObject.AddComponent<BlobSite>());
            }

            newNode.Terrain = startingTerrain;
            newNode.transform.localPosition = localPosition;
            newNode.transform.SetParent(this.transform, false);

            SubscribeNode(newNode);

            return newNode;
        }

        /// <inheritdoc/>
        public override void DestroyNode(MapNodeBase node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }
            UnsubscribeNode(node);
            DestroyImmediate(node.gameObject);
        }

        /// <inheritdoc/>
        public override void SubscribeNode(MapNodeBase node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }
            if(!nodes.Contains(node)) {
                nodes.Add(node);

                node.ParentGraph = this;
                node.UIControl = UIControl;
                node.BlobSite.Configuration = BlobSiteConfiguration;
                node.TerrainMaterialRegistry = TerrainMaterialRegistry;
                node.TerrainGrid = TerrainTileGrid;

                node.TransformChanged += Node_TransformChanged;

                node.name = string.Format("Node [{0}]", node.ID);

                node.ClearAssociatedTiles();
            }
        }

        /// <inheritdoc/>
        public override void UnsubscribeNode(MapNodeBase nodeToRemove) {
            if(nodeToRemove == null) {
                throw new ArgumentNullException("node");
            }

            nodeToRemove.TransformChanged -= Node_TransformChanged;

            bool existedInGraph = nodes.Remove(nodeToRemove);
            if(existedInGraph && edges != null) {
                var edgesToRemove = new List<MapEdgeBase>(edges.Where(delegate(MapEdgeBase edge){
                    return edge.FirstNode == nodeToRemove || edge.SecondNode == nodeToRemove;
                }));
                foreach(var edge in edgesToRemove){
                    if(edge != null) {
                        UnsubscribeMapEdge(edge);
                    }
                }
            }
            nodeToRemove.ParentGraph = null;
            nodeToRemove.UIControl = null;
            nodeToRemove.TerrainMaterialRegistry = null;
            if(nodeToRemove.BlobSite != null) {
                nodeToRemove.BlobSite.Configuration = null;
            }

            RaiseMapNodeUnsubscribed(nodeToRemove);
        }

        /// <inheritdoc/>
        public override MapEdgeBase BuildMapEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }else if(GetEdge(first, second) != null) {
                throw new MapGraphException("There already exists and edge between these two MapNodes");
            }

            MapEdge newEdge;
            if(EdgePrefab != null) {
                var edgePrefabClone = Instantiate(EdgePrefab);
                newEdge = edgePrefabClone.GetComponent<MapEdge>();
            }else {
                var hostingObject = new GameObject();
                newEdge = hostingObject.AddComponent<MapEdge>();
                newEdge.DisplayComponent = new GameObject().transform;
            }

            newEdge.transform.position += Vector3.forward;

            newEdge.SetNodes(first, second);

            newEdge.gameObject.name = string.Format("Edge [{0}]", newEdge.ID);

            newEdge.transform.SetParent(first.transform.parent);
            SubscribeMapEdge(newEdge);

            return newEdge;
        }

        /// <inheritdoc/>
        public override void DestroyMapEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }

            var edgeToDestroy = GetEdge(first, second);
            if(edgeToDestroy != null) {
                DestroyMapEdge(edgeToDestroy);
            }
            
        }

        /// <inheritdoc/>
        public override void DestroyMapEdge(MapEdgeBase edge) {
            if(edge == null) {
                throw new ArgumentNullException("edge");
            }
            UnsubscribeMapEdge(edge);

            DestroyImmediate(edge.gameObject);
        }

        /// <inheritdoc/>
        public override void SubscribeMapEdge(MapEdgeBase edge) {
            if(edge == null) {
                throw new ArgumentNullException("edge");
            }
            if(!edges.Contains(edge)) {
                edges.Add(edge);

                NeighborsOfNode.AddElementToList(edge.FirstNode,  edge.SecondNode);
                NeighborsOfNode.AddElementToList(edge.SecondNode, edge.FirstNode );
                edge.ParentGraph = this;
                edge.gameObject.SetActive(true);
            }
        }

        /// <inheritdoc/>
        public override void UnsubscribeMapEdge(MapEdgeBase edge) {
            if(edge == null) {
                throw new ArgumentNullException("edge");
            }
            edges.Remove(edge);
            if(NeighborsOfNode.ContainsKey(edge.FirstNode)) {
                NeighborsOfNode[edge.FirstNode ].Remove(edge.SecondNode);
            }
            if(NeighborsOfNode.ContainsKey(edge.SecondNode)) {
                NeighborsOfNode[edge.SecondNode].Remove(edge.FirstNode );
            }
            edge.ParentGraph = null;
            RaiseMapEdgeUnsubscribed(edge);
        }

        /// <inheritdoc/>
        public override MapNodeBase GetNodeOfID(int id) {
            return nodes.Find(node => node.ID == id);
        }

        /// <inheritdoc/>
        public override MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }
            var validEdges = edges.Where(ConstructEdgeExistsTest(first, second));
            return validEdges.FirstOrDefault();
        }

        /// <inheritdoc/>
        public  override IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }
            List<MapNodeBase> neighbors;
            NeighborsOfNode.TryGetValue(node, out neighbors);
            if(neighbors == null) {
                return new List<MapNodeBase>();
            }else {
                return neighbors;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }
            var retval = new List<MapEdgeBase>();
            foreach(var neighbor in GetNeighborsOfNode(node)) {
                retval.Add(GetEdge(node, neighbor));
            }
            return retval;
        }

        /// <inheritdoc/>
        public override int GetDistanceBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
            if(nodeOne == null) {
                throw new ArgumentNullException("node1");
            }else if(nodeTwo == null) {
                throw new ArgumentNullException("node2");
            }
            return AlgorithmSet.GetDistanceBetweenNodes(nodeOne, nodeTwo, Nodes);
        }

        /// <inheritdoc/>
        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
            if(nodeOne == null) {
                throw new ArgumentNullException("start");
            }else if(nodeTwo == null) {
                throw new ArgumentNullException("end");
            }
            return AlgorithmSet.GetShortestPathBetweenNodes(nodeOne, nodeTwo, Nodes);
        }

        /// <inheritdoc/>
        public override NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin,
            Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue) {
            if(edgeOfOrigin == null) {
                throw new ArgumentNullException("edgeOfOrigin");
            }else if(condition == null) {
                throw new ArgumentNullException("condition");
            }
            return AlgorithmSet.GetNearestNodeToEdgeWhere(edgeOfOrigin, condition, maxDistance);
        }

        #endregion

        private Func<MapEdgeBase, bool> ConstructEdgeExistsTest(MapNodeBase first, MapNodeBase second) {
            return delegate(MapEdgeBase edge) {
                return (
                    ( edge.FirstNode  == first && edge.SecondNode == second ) ||
                    ( edge.SecondNode == first && edge.FirstNode  == second )
                );
            };
        }

        private void Node_TransformChanged(object sender, EventArgs e) {
            var node = sender as MapNodeBase;
            foreach(var edge in GetEdgesAttachedToNode(node)) {
                edge.RefreshOrientation();
            }
        }

        #endregion

    }

}
