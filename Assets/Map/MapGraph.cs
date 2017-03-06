using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Blobs;

namespace Assets.Map {

    public class MapGraph : MonoBehaviour {

        #region instance fields and properties

        public ReadOnlyCollection<MapNode> Nodes {
            get { return NodeSet.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<MapNode> NodeSet = new List<MapNode>();

        public ReadOnlyCollection<MapEdge> Edges {
            get { return EdgeSet.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<MapEdge> EdgeSet = new List<MapEdge>();

        [SerializeField] private GameObject NodePrefab;

        private DictionaryOfLists<MapNode, MapNode> NeighborsOfNode {
            get {
                if(_neighborsOfNode == null) {
                    _neighborsOfNode = new DictionaryOfLists<MapNode, MapNode>();
                    foreach(var edge in EdgeSet) {
                        _neighborsOfNode.AddElementToList(edge.FirstNode, edge.SecondNode);
                        _neighborsOfNode.AddElementToList(edge.SecondNode, edge.FirstNode);
                    }
                }
                return _neighborsOfNode;
            }
        }
        private DictionaryOfLists<MapNode, MapNode> _neighborsOfNode = null;

        #endregion

        #region instance methods

        public MapNode BuildNode(Vector3 localPosition) {
            var nodeObject = Instantiate(NodePrefab, this.transform, false) as GameObject;
            var nodeBehaviour = nodeObject.GetComponent<MapNode>();
            if(nodeBehaviour != null) {
                SubscribeNode(nodeBehaviour);
                nodeBehaviour.transform.localPosition = localPosition;
                return nodeBehaviour;
            }else {
                throw new MapGraphException("The NodePrefab lacks a MapNode component");
            }
        }

        public void SubscribeNode(MapNode node) {
            if(node == null) {
                throw new ArgumentNullException("node");
            }else if(!NodeSet.Contains(node)){
                node.transform.SetParent(this.transform, false);
                node.ManagingGraph = this;
                NodeSet.Add(node);
            }else {
                throw new MapGraphException("This node has already been subscribed to this MapGraph");
            }
        }

        public void AddUndirectedEdge(MapNode first, MapNode second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }else if(HasEdge(first, second)) {
                throw new MapGraphException("There already exists and edge between these two MapNodes");
            }
            EdgeSet.Add(new MapEdge(first, second));
            NeighborsOfNode.AddElementToList(first, second);
            NeighborsOfNode.AddElementToList(second, first);
        }

        public bool RemoveUndirectedEdge(MapNode first, MapNode second) {
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

        public bool RemoveUndirectedEdge(MapEdge edge) {
            var retval = EdgeSet.Remove(edge);
            if(NeighborsOfNode.ContainsKey(edge.FirstNode)) {
                NeighborsOfNode[edge.FirstNode ].Remove(edge.SecondNode);
            }
            if(NeighborsOfNode.ContainsKey(edge.SecondNode)) {
                NeighborsOfNode[edge.SecondNode].Remove(edge.FirstNode );
            }
            return retval;
        }

        public bool RemoveNode(MapNode nodeToRemove) {
            if(nodeToRemove == null) {
                throw new ArgumentNullException("node");
            }
            bool existedInGraph = NodeSet.Remove(nodeToRemove);
            if(existedInGraph) {
                var edgesToRemove = new List<MapEdge>(EdgeSet.Where(delegate(MapEdge edge){
                    return edge.FirstNode == nodeToRemove || edge.SecondNode == nodeToRemove;
                }));
                foreach(var edge in edgesToRemove){
                    RemoveUndirectedEdge(edge);
                }
            }
            
            return existedInGraph;
        }

        public MapNode GetNodeOfID(int id) {
            throw new NotImplementedException();
        }

        public bool HasEdge(MapNode first, MapNode second) {
            if(first == null) {
                throw new ArgumentNullException("first");
            }else if(second == null) {
                throw new ArgumentNullException("second");
            }
            return EdgeSet.Where(ConstructEdgeExistsTest(first, second)).Count() > 0;
        }

        public MapEdge GetEdge(MapNode first, MapNode second) {
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

        public IEnumerable<MapNode> GetNeighborsOfNode(MapNode node) {
            List<MapNode> neighbors;
            NeighborsOfNode.TryGetValue(node, out neighbors);
            if(neighbors == null) {
                return new List<MapNode>();
            }else {
                return neighbors;
            }
        }

        public IEnumerable<MapEdge> GetEdgesAttachedToNode(MapNode node) {
            var retval = new List<MapEdge>();
            foreach(var neighbor in GetNeighborsOfNode(node)) {
                retval.Add(GetEdge(node, neighbor));
            }
            return retval;
        }

        private Func<MapEdge, bool> ConstructEdgeExistsTest(MapNode first, MapNode second) {
            return delegate(MapEdge edge) {
                return (
                    ( edge.FirstNode  == first && edge.SecondNode == second ) ||
                    ( edge.SecondNode == first && edge.FirstNode  == second )
                );
            };
        }

        #endregion

    }

}
