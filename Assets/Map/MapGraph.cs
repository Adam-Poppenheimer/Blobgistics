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
        private List<MapNodeBase> nodes = new List<MapNodeBase>();

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get { return edges.AsReadOnly(); }
        }
        private List<MapEdgeBase> edges = new List<MapEdgeBase>();

        #endregion

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

        public BlobSiteConfigurationBase BlobSiteConfiguration {
            get { return _blobSiteConfiguration; }
            set { _blobSiteConfiguration = value; }
        }
        [SerializeField] private BlobSiteConfigurationBase _blobSiteConfiguration;

        [SerializeField] private GameObject NodePrefab;
        [SerializeField] private GameObject EdgePrefab;

        private DictionaryOfLists<MapNodeBase, MapNodeBase> NeighborsOfNode = 
            new DictionaryOfLists<MapNodeBase, MapNodeBase>();

        #endregion

        #region instance methods

        #region Unity event methods

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
                newNode.SetBlobSite(newNode.gameObject.AddComponent<BlobSite>());
            }

            newNode.Terrain = startingTerrain;
            newNode.transform.localPosition = localPosition;
            newNode.transform.SetParent(this.transform);

            SubscribeNode(newNode);

            return newNode;
        }

        public override void DestroyNode(MapNodeBase node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }
            UnsubscribeNode(node);
            DestroyImmediate(node.gameObject);
        }

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

                node.TransformChanged += Node_TransformChanged;

                node.name = string.Format("Node [{0}]", node.ID);
            }
        }

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
                newEdge.SetBlobSite(newEdge.gameObject.AddComponent<BlobSite>());
            }

            newEdge.transform.position += Vector3.forward;

            newEdge.SetNodes(first, second);

            newEdge.gameObject.name = string.Format("Edge [{0}]", newEdge.ID);

            newEdge.transform.SetParent(first.transform.parent);
            SubscribeMapEdge(newEdge);

            return newEdge;
        }

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

        public override void DestroyMapEdge(MapEdgeBase edge) {
            if(edge == null) {
                throw new ArgumentNullException("edge");
            }
            UnsubscribeMapEdge(edge);

            DestroyImmediate(edge.gameObject);
        }

        public override void SubscribeMapEdge(MapEdgeBase edge) {
            if(edge == null) {
                throw new ArgumentNullException("edge");
            }
            if(!edges.Contains(edge)) {
                edges.Add(edge);

                NeighborsOfNode.AddElementToList(edge.FirstNode,  edge.SecondNode);
                NeighborsOfNode.AddElementToList(edge.SecondNode, edge.FirstNode );
                edge.ParentGraph = this;
                if(edge.BlobSite != null) {
                    edge.BlobSite.Configuration = BlobSiteConfiguration;
                }
                edge.gameObject.SetActive(true);
            }
        }

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
            if(edge.BlobSite != null) {
                edge.BlobSite.Configuration = null;
            }
            RaiseMapEdgeUnsubscribed(edge);
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

        private void Node_TransformChanged(object sender, EventArgs e) {
            var node = sender as MapNodeBase;
            foreach(var edge in GetEdgesAttachedToNode(node)) {
                edge.RefreshOrientation();
            }
        }

        #endregion

    }

}
