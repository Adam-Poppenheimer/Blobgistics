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

        public MapGraphAlgorithmSetBase AlgorithmSet { get; set; }

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override MapEdgeBase BuildMapEdge(MapNodeBase first, MapNodeBase second) {
            var hostingObject = new GameObject();
            var newEdge = hostingObject.AddComponent<MockMapEdge>();
            newEdge.firstNode = first;
            newEdge.secondNode = second;
            edges.Add(newEdge);
            Neighbors.AddElementToList(first, second);
            Neighbors.AddElementToList(second, first);

            return newEdge;
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

        public override void UnsubscribeNode(MapNodeBase nodeToRemove) {
            throw new NotImplementedException();
        }

        public override void DestroyMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void DestroyMapEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void SubscribeNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            return AlgorithmSet.GetDistanceBetweenNodes(node1, node2, nodes);
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            return AlgorithmSet.GetShortestPathBetweenNodes(node1, node2, nodes);
        }

        public override NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin,
            Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue) {

            return AlgorithmSet.GetNearestNodeToEdgeWhere(edgeOfOrigin, condition, maxDistance);
        }

        public override void DestroyNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override void SubscribeMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
