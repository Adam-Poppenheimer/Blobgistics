using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Map;
using UnityEngine;

namespace Assets.Core.ForTesting {

    public class MockMapGraph : MapGraphBase {

        #region instance fields and properties

        #region from MapGraphBase

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get { return edges.AsReadOnly(); }
        }
        private List<MapEdgeBase> edges = new List<MapEdgeBase>();

        public override ReadOnlyCollection<MapNodeBase> Nodes {
            get { return nodes.AsReadOnly(); }
        }
        private List<MapNodeBase> nodes = new List<MapNodeBase>();

        #endregion

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override MapEdgeBase BuildUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            var newEdge = (new GameObject()).AddComponent<MockMapEdge>();
            newEdge.firstNode = first;
            newEdge.secondNode = second;

            edges.Add(newEdge);

            return newEdge;
        }

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            var newNode = (new GameObject()).AddComponent<MockMapNode>();
            nodes.Add(newNode);
            return newNode;
        }

        public override MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain) {
            throw new NotImplementedException();
        }

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            throw new NotImplementedException();
        }

        public override MapEdgeBase GetEdge(MapNodeBase first, MapNodeBase second) {
            return edges.Where(delegate(MapEdgeBase edge) {
                return (
                    (edge.FirstNode  == first && edge.SecondNode == second) ||
                    (edge.SecondNode == first && edge.FirstNode  == second)
                );
            }).FirstOrDefault();
        }

        public override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override MapNodeBase GetNodeOfID(int id) {
            return nodes.Where(node => node.ID == id).FirstOrDefault();
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase node1, MapNodeBase node2) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeNode(MapNodeBase nodeToRemove) {
            throw new NotImplementedException();
        }

        public override void DestroyUndirectedEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void DestroyUndirectedEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeDirectedEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void SubscribeNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin,
            Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue) {
            throw new NotImplementedException();
        }

        public override void DestroyNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override void SubscribeUndirectedEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
