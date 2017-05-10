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
            get { return NodeSet.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<MapNodeBase> NodeSet = new List<MapNodeBase>();

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get { return EdgeSet.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<MapEdgeBase> EdgeSet = new List<MapEdgeBase>();

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

        [SerializeField] private GameObject NodePrefab;
        [SerializeField] private GameObject EdgePrefab;

        private MapNodeShortestPathLogicBase ShortestPathLogic = MapNodeShortestPathLogic.Instance;

        private DictionaryOfLists<MapNodeBase, MapNodeBase> NeighborsOfNode {
            get {
                if(_neighborsOfNode == null) {
                    _neighborsOfNode = new DictionaryOfLists<MapNodeBase, MapNodeBase>();
                    foreach(var edge in EdgeSet) {
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
            newNode.SetManagingGraph(this);
            newNode.SetBlobSite(BlobSiteFactory.ConstructBlobSite(newNode.gameObject));
            newNode.UIControl = UIControl;

            newNode.TerrainMaterialRegistry = TerrainMaterialRegistry;
            newNode.Terrain = startingTerrain;

            SubscribeNode(newNode);
            return newNode;
        }

        public override void SubscribeNode(MapNodeBase node) {
            if(!NodeSet.Contains(node)) {
                NodeSet.Add(node);
                node.name = string.Format("Node [{0}]", node.ID);
            }
        }

        public override void AddUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }else if(HasEdge(first, second)) {
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
            newEdge.SetParentGraph(this);
            outerHost.gameObject.name = string.Format("Edge [{0}]", newEdge.ID);
            newEdge.gameObject.name = string.Format("Edge [{0}]", newEdge.ID);

            EdgeSet.Add(newEdge);
            NeighborsOfNode.AddElementToList(first, second);
            NeighborsOfNode.AddElementToList(second, first);
        }

        public override bool RemoveUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }

            if(HasEdge(first, second)) {
                return RemoveUndirectedEdge(GetEdge(first, second));
            }else {
                return false;
            }
            
        }

        public override bool RemoveUndirectedEdge(MapEdgeBase edge) {
            var retval = UnsubscribeDirectedEdge(edge);
            if(Application.isPlaying) {
                Destroy(edge.transform.parent.gameObject);
            }else {
                DestroyImmediate(edge.transform.parent.gameObject);
            }
            return retval;
        }

        public override bool UnsubscribeDirectedEdge(MapEdgeBase edge) {
            var retval = EdgeSet.Remove(edge);
            if(NeighborsOfNode.ContainsKey(edge.FirstNode)) {
                NeighborsOfNode[edge.FirstNode ].Remove(edge.SecondNode);
            }
            if(NeighborsOfNode.ContainsKey(edge.SecondNode)) {
                NeighborsOfNode[edge.SecondNode].Remove(edge.FirstNode );
            }
            return retval;
        }

        public override bool RemoveNode(MapNodeBase nodeToRemove) {
            if(nodeToRemove == null) {
                throw new ArgumentNullException("node");
            }
            bool existedInGraph = NodeSet.Remove(nodeToRemove);
            if(existedInGraph && EdgeSet != null) {
                var edgesToRemove = new List<MapEdgeBase>(EdgeSet.Where(delegate(MapEdgeBase edge){
                    return edge.FirstNode == nodeToRemove || edge.SecondNode == nodeToRemove;
                }));
                foreach(var edge in edgesToRemove){
                    RemoveUndirectedEdge(edge);
                }
            }

            return existedInGraph;
        }

        public override MapNodeBase GetNodeOfID(int id) {
            return NodeSet.Find(node => node.ID == id);
        }

        public  override bool HasEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }
            return EdgeSet.Where(ConstructEdgeExistsTest(first, second)).Count() > 0;
        }

        public override MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }else if(!HasEdge(first, second)) {
                throw new MapGraphException("This MapGraph has no edge between the specified MapNodes");
            }
            var validEdges = EdgeSet.Where(ConstructEdgeExistsTest(first, second));
            return validEdges.First();
        }

        public  override IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node) {
            List<MapNodeBase> neighbors;
            NeighborsOfNode.TryGetValue(node, out neighbors);
            if(neighbors == null) {
                return new List<MapNodeBase>();
            }else {
                return neighbors;
            }
        }

        public override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            var retval = new List<MapEdgeBase>();
            foreach(var neighbor in GetNeighborsOfNode(node)) {
                retval.Add(GetEdge(node, neighbor));
            }
            return retval;
        }

        public override List<NodeDistanceSummary> GetNodesWithinDistanceOfEdge(MapEdgeBase edge, uint distanceInEdges) {
            var retval = new List<NodeDistanceSummary>();
            foreach(var nodeToCheck in Nodes) {
                int distanceFromFirst = GetDistanceBetweenNodes(edge.FirstNode, nodeToCheck);
                int distanceFromSecond = GetDistanceBetweenNodes(edge.SecondNode, nodeToCheck);
                if(distanceFromFirst <= distanceInEdges || distanceFromSecond <= distanceInEdges) {
                    retval.Add(new NodeDistanceSummary(nodeToCheck, Math.Min(distanceFromFirst, distanceFromSecond)));
                }
            }
            return retval;
        }

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            return ShortestPathLogic.GetDistanceBetweenNodes(node1, node2, Nodes);
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase start, MapNodeBase end) {
            return ShortestPathLogic.GetShortestPathBetweenNodes(start, end, Nodes);
        }

        public override NodeDistanceSummary GetNearestNodeWhere(MapEdgeBase edgeOfOrigin,
            Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue) {

            return ShortestPathLogic.GetNearestNodeWhere(edgeOfOrigin, condition, maxDistance);
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
