using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager.ForTesting {

    public class MockMapGraph : MapGraphBase {

        #region instance fields and properties

        #region from MapGraphBase

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get { return edges.AsReadOnly(); }
        }
        private List<MapEdgeBase> edges = new List<MapEdgeBase>();

        public override ReadOnlyCollection<MapNodeBase> Nodes {
            get {
                throw new NotImplementedException();
            }
        }
        private List<MapNodeBase> nodes = new List<MapNodeBase>();

        private DictionaryOfLists<MapNodeBase, MapNodeBase> Neighbors =
            new DictionaryOfLists<MapNodeBase, MapNodeBase>();

        #endregion

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override void AddUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            var hostingObject = new GameObject();
            var newEdge = hostingObject.AddComponent<MockMapEdge>();
            newEdge.firstNode = first;
            newEdge.secondNode = second;
            edges.Add(newEdge);
            Neighbors.AddElementToList(first, second);
            Neighbors.AddElementToList(second, first);
        }

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            var hostingObject = new GameObject();
            var newNode = hostingObject.AddComponent<MockMapNode>();
            newNode.managingGraph = this;
            nodes.Add(newNode);
            return newNode;
        }

        public override MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain) {
            throw new NotImplementedException();
        }

        public override MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second) {
            return edges.Find(delegate(MapEdgeBase edge) {
                return (edge.FirstNode == first  && edge.SecondNode == second) ||
                    (edge.FirstNode == second && edge.SecondNode == first);
            });
        }

        public override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node) {
            return Neighbors[node];
        }

        public override MapNodeBase GetNodeOfID(int id) {
            throw new NotImplementedException();
        }

        public override bool HasEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override bool RemoveNode(MapNodeBase nodeToRemove) {
            throw new NotImplementedException();
        }

        public override bool RemoveUndirectedEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override bool RemoveUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override bool UnsubscribeDirectedEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void SubscribeNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override List<NodeDistanceSearchResults> GetNodesWithinDistanceOfEdge(MapEdgeBase edge, uint distanceInEdges) {
            var retval = new List<NodeDistanceSearchResults>();
            foreach(var nodeToCheck in nodes) {
                int distanceFromFirst = GetDistanceBetweenNodes(edge.FirstNode, nodeToCheck);
                int distanceFromSecond = GetDistanceBetweenNodes(edge.SecondNode, nodeToCheck);
                if(distanceFromFirst <= distanceInEdges || distanceFromSecond <= distanceInEdges) {
                    retval.Add(new NodeDistanceSearchResults(nodeToCheck, Math.Min(distanceFromFirst, distanceFromSecond)));
                }
            }
            return retval;
        }

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            return MapNodeShortestPathLogic.Instance.GetDistanceBetweenNodes(node1, node2, nodes);
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            return MapNodeShortestPathLogic.Instance.GetShortestPathBetweenNodes(node1, node2, nodes);
        }

        #endregion

        #endregion

    }

}
