using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using System.Collections.ObjectModel;

namespace Assets.Session.ForTesting {

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

        public override MapEdgeBase BuildMapEdge(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            var newEdge = (new GameObject()).AddComponent<MockMapEdge>();
            newEdge.firstNode = firstEndpoint;
            newEdge.secondNode = secondEndpoint;
            edges.Add(newEdge);
            return newEdge;
        }

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            return BuildNode(localPosition, TerrainType.Grassland);
        }

        public override MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain) {
            var newNode = (new GameObject()).AddComponent<MockMapNode>();
            newNode.transform.SetParent(this.transform);
            newNode.transform.localPosition = localPosition;
            newNode.Terrain = startingTerrain;
            nodes.Add(newNode);
            return newNode;
        }

        public override void DestroyMapEdge(MapEdgeBase edge) {
            edges.Remove(edge);
            DestroyImmediate(edge.gameObject);
        }

        public override void DestroyMapEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override void DestroyNode(MapNodeBase node) {
            nodes.Remove(node);
            DestroyImmediate(node.gameObject);
        }

        public override int GetDistanceBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
            throw new NotImplementedException();
        }

        public override MapEdgeBase GetEdge(MapNodeBase endpointOne, MapNodeBase endpointTwo) {
            throw new NotImplementedException();
        }

        public override IEnumerable<MapEdgeBase> GetEdgesAttachedToNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin, Predicate<MapNodeBase> condition, int maxDistance = int.MaxValue) {
            throw new NotImplementedException();
        }

        public override IEnumerable<MapNodeBase> GetNeighborsOfNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override MapNodeBase GetNodeOfID(int id) {
            throw new NotImplementedException();
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase nodeOne, MapNodeBase nodeTwo) {
            throw new NotImplementedException();
        }

        public override void LoadFromMapAsset(MapAsset asset) {
            throw new NotImplementedException();
        }

        public override void SubscribeMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void SubscribeNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeNode(MapNodeBase node) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
