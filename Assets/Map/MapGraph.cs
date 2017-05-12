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

    [ExecuteInEditMode]
    public class MapGraph : MapGraphBase {

        #region instance fields and properties

        #region from MapGraphBase

        public override ReadOnlyCollection<MapNodeBase> Nodes {
            get { return nodes.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<MapNodeBase> nodes = new List<MapNodeBase>();

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get { return edges.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<MapEdgeBase> edges = new List<MapEdgeBase>();

        #endregion

        public BlobSiteFactoryBase BlobSiteFactory {
            get {
                if(_blobSiteFactory == null) {
                    throw new InvalidOperationException("BlobSiteFactory is uninitialized");
                } else {
                    return _blobSiteFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _blobSiteFactory = value;
                }
            }
        }
        [SerializeField] private BlobSiteFactoryBase _blobSiteFactory;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        public TerrainMaterialRegistry TerrainMaterialRegistry {
            get { return _terrainMaterialRegistry; }
            set { _terrainMaterialRegistry = value; }
        }
        [SerializeField] private TerrainMaterialRegistry _terrainMaterialRegistry;

        public MapGraphAlgorithmSetBase AlgorithmSet {
            get { return _algorithmSet; }
            set { _algorithmSet = value; }
        }
        [SerializeField] private MapGraphAlgorithmSetBase _algorithmSet;

        [SerializeField] private GameObject NodePrefab;
        [SerializeField] private GameObject EdgePrefab;

        private DictionaryOfLists<MapNodeBase, MapNodeBase> NeighborsOfNode {
            get {
                if(_neighborsOfNode == null) {
                    _neighborsOfNode = new DictionaryOfLists<MapNodeBase, MapNodeBase>();
                    foreach(var edge in edges) {
                        _neighborsOfNode.AddElementToList(edge.FirstNode, edge.SecondNode);
                        _neighborsOfNode.AddElementToList(edge.SecondNode, edge.FirstNode);
                    }
                }
                return _neighborsOfNode;
            }
        }
        private DictionaryOfLists<MapNodeBase, MapNodeBase> _neighborsOfNode = null;

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            return BuildNode(localPosition, TerrainType.Grassland);
        }

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
            }

            newNode.transform.SetParent(this.transform, false);
            newNode.transform.localPosition = localPosition;
            newNode.SetParentGraph(this);
            newNode.SetBlobSite(BlobSiteFactory.ConstructBlobSite(newNode.gameObject));
            newNode.UIControl = UIControl;

            newNode.TerrainMaterialRegistry = TerrainMaterialRegistry;
            newNode.Terrain = startingTerrain;

            SubscribeNode(newNode);
            return newNode;
        }

        public override void DestroyNode(MapNodeBase node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }
            nodes.Remove(node);
            if(Application.isPlaying) {
                Destroy(node.gameObject);
            }else {
                DestroyImmediate(node.gameObject);
            }
        }

        public override void SubscribeNode(MapNodeBase node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }
            if(!nodes.Contains(node)) {
                nodes.Add(node);
                node.name = string.Format("Node [{0}]", node.ID);
            }
        }

        public override void UnsubscribeNode(MapNodeBase nodeToRemove) {
            if(nodeToRemove == null) {
                throw new ArgumentNullException("node");
            }
            bool existedInGraph = nodes.Remove(nodeToRemove);
            if(existedInGraph && edges != null) {
                var edgesToRemove = new List<MapEdgeBase>(edges.Where(delegate(MapEdgeBase edge){
                    return edge.FirstNode == nodeToRemove || edge.SecondNode == nodeToRemove;
                }));
                foreach(var edge in edgesToRemove){
                    DestroyUndirectedEdge(edge);
                }
            }
            (nodeToRemove as MapNode).SetParentGraph(null);
        }

        public override MapEdgeBase BuildUndirectedEdge(MapNodeBase first, MapNodeBase second) {
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
            }

            var outerHost = new GameObject();

            newEdge.transform.SetParent(outerHost.transform, false);
            outerHost.transform.SetParent(this.transform, false);
            EdgeOrientationUtil.AlignTransformWithEndpoints(outerHost.transform, first.transform.position, second.transform.position, false);
            EdgeOrientationUtil.AlignTransformWithEndpoints(newEdge.transform, first.transform.position, second.transform.position, true);
            newEdge.transform.position += Vector3.forward;

            newEdge.SetFirstNode(first);
            newEdge.SetSecondNode(second);
            newEdge.SetBlobSite(BlobSiteFactory.ConstructBlobSite(outerHost.gameObject));

            outerHost.gameObject.name = string.Format("Edge [{0}]", newEdge.ID);
            newEdge.gameObject.name   = string.Format("Edge [{0}]", newEdge.ID);

            SubscribeUndirectedEdge(newEdge);

            return newEdge;
        }

        public override void DestroyUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }

            var edgeToDestroy = GetEdge(first, second);
            if(edgeToDestroy != null) {
                DestroyUndirectedEdge(edgeToDestroy);
            }
            
        }

        public override void DestroyUndirectedEdge(MapEdgeBase edge) {
            if(edge == null) {
                throw new ArgumentNullException("edge");
            }
            UnsubscribeDirectedEdge(edge);
            var objectToDestroy = edge.transform.parent != null ? edge.transform.parent.gameObject : edge.gameObject;

            if(Application.isPlaying) {
                Destroy(objectToDestroy);
            }else {
                DestroyImmediate(objectToDestroy);
            }
        }

        public override void SubscribeUndirectedEdge(MapEdgeBase edge) {
            if(edge == null) {
                throw new ArgumentNullException("edge");
            }
            edges.Add(edge);
            NeighborsOfNode.AddElementToList(edge.FirstNode,  edge.SecondNode);
            NeighborsOfNode.AddElementToList(edge.SecondNode, edge.FirstNode );
            edge.ParentGraph = this;
        }

        public override void UnsubscribeDirectedEdge(MapEdgeBase edge) {
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
            (edge as MapEdge).ParentGraph = null;
        }

        public override MapNodeBase GetNodeOfID(int id) {
            return nodes.Find(node => node.ID == id);
        }

        public override MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }
            var validEdges = edges.Where(ConstructEdgeExistsTest(first, second));
            return validEdges.FirstOrDefault();
        }

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

        public override int GetDistanceBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
            if(nodeOne == null) {
                throw new ArgumentNullException("node1");
            }else if(nodeTwo == null) {
                throw new ArgumentNullException("node2");
            }
            return AlgorithmSet.GetDistanceBetweenNodes(nodeOne, nodeTwo, Nodes);
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
            if(nodeOne == null) {
                throw new ArgumentNullException("start");
            }else if(nodeTwo == null) {
                throw new ArgumentNullException("end");
            }
            return AlgorithmSet.GetShortestPathBetweenNodes(nodeOne, nodeTwo, Nodes);
        }

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

        #endregion

    }

}
