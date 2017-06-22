using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Map.ForTesting {

    public class MockMapGraph : MapGraphBase {

        #region instance fields and properties

        #region from MapGraphBase

        public override ReadOnlyCollection<MapEdgeBase> Edges {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<MapNodeBase> Nodes {
            get { return _nodes.AsReadOnly(); }
        }
        private List<MapNodeBase> _nodes = new List<MapNodeBase>();

        #endregion

        #endregion

        #region instance methods

        #region from MapGraphBase

        public override MapEdgeBase BuildMapEdge(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            throw new NotImplementedException();
        }

        public override MapNodeBase BuildNode(Vector3 localPosition) {
            throw new NotImplementedException();
        }

        public override MapNodeBase BuildNode(Vector3 localPosition, TerrainType startingTerrain) {
            throw new NotImplementedException();
        }

        public override void DestroyMapEdge(MapEdgeBase edge) {
            throw new NotImplementedException();
        }

        public override void DestroyMapEdge(MapNodeBase first, MapNodeBase second) {
            throw new NotImplementedException();
        }

        public override void DestroyNode(MapNodeBase node) {
            throw new NotImplementedException();
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
