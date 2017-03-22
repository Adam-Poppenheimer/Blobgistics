using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.BlobSites;
using Assets.Core;

namespace Assets.Map {

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

        [SerializeField] private GameObject NodePrefab;

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

        public override MapNodeBase BuildNode(Vector3 localPosition) {
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
            var hostingObject = new GameObject();
            hostingObject.transform.SetParent(this.transform);
            hostingObject.transform.position = (first.transform.position + second.transform.position) / 2;

            var newEdge = hostingObject.AddComponent<MapEdge>();
            newEdge.SetFirstNode(first);
            newEdge.SetSecondNode(second);
            newEdge.SetBlobSite(BlobSiteFactory.ConstructBlobSite(hostingObject));
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
            var retval = EdgeSet.Remove(edge);
            if(NeighborsOfNode.ContainsKey(edge.FirstNode)) {
                NeighborsOfNode[edge.FirstNode ].Remove(edge.SecondNode);
            }
            if(NeighborsOfNode.ContainsKey(edge.SecondNode)) {
                NeighborsOfNode[edge.SecondNode].Remove(edge.FirstNode );
            }
            DestroyImmediate(edge.gameObject);
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

        public  override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            var retval = new List<MapEdgeBase>();
            foreach(var neighbor in GetNeighborsOfNode(node)) {
                retval.Add(GetEdge(node, neighbor));
            }
            return retval;
        }

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
