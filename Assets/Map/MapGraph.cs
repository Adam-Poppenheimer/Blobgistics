using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    public class MapGraph : MonoBehaviour {

        #region instance fields and properties

        public ReadOnlyCollection<MapNode> Nodes {
            get { return NodeSet.AsReadOnly(); }
        }
        [SerializeField] private List<MapNode> NodeSet = new List<MapNode>();

        #endregion

        #region instance methods

        public void AddNode(MapNode node) {
            if(!NodeSet.Contains(node)) {
                NodeSet.Add(node);
            }else {
                throw new MapGraphException("Cannot add node to MapGraph; node already exists");
            }
        }

        public bool TryAddNode(MapNode node) {
            if(!NodeSet.Contains(node)) {
                AddNode(node);
                return true;
            }else {
                return false;
            }
        }

        public void AddUndirectedEdge(MapNode from, MapNode to) {
            from.AddNeighbor(to);
            to.AddNeighbor(from);
        }

        public bool RemoveUndirectedEdge(MapNode from, MapNode to) {
            from.RemoveNeighbor(to);
            return to.RemoveNeighbor(from);
        }

        public bool Remove(MapNode nodeToRemove) {
            if(nodeToRemove == null) {
                throw new ArgumentNullException("node");
            }
            bool existedInGraph = NodeSet.Remove(nodeToRemove);
            if(existedInGraph) {
                var oldNeighbors = new List<MapNode>(nodeToRemove.Neighbors);
                foreach(var oldNeighbor in oldNeighbors) {
                    oldNeighbor.RemoveNeighbor(nodeToRemove);
                    nodeToRemove.RemoveNeighbor(oldNeighbor);
                }
            }
            
            return existedInGraph;
        }

        public bool HasEdge(MapNode from, MapNode to) {
            return from.Neighbors.Contains(to);
        }

        public float GetEdgeWeight(MapNode from, MapNode to) {
            if(HasEdge(from, to)) {
                return Vector3.Distance(from.transform.position, to.transform.position);
            }else {
                throw new MapGraphException("This graph does not possess this edge");
            }
        }

        #endregion

    }

}
