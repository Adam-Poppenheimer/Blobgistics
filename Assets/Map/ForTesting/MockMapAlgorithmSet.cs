using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map.ForTesting {

    public class MockMapAlgorithmSet : MapGraphAlgorithmSetBase {

        #region events

        public event Action<MapNodeBase, MapNodeBase, IEnumerable<MapNodeBase>> GetDistanceBetweenNodesCalled;

        public event Action<MapEdgeBase, Predicate<MapNodeBase>, int> GetNearestNodeToEdgeWhereCalled;

        public event Action<MapNodeBase, Predicate<MapNodeBase>, int> GetNearestNodeToNodeWhereCalled;

        public event Action<MapNodeBase, MapNodeBase, IEnumerable<MapNodeBase>> GetShortestPathBetweenNodesCalled;

        #endregion

        #region instance methods

        #region from MapGraphAlgorithmSetBase

        public override int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2, IEnumerable<MapNodeBase> allNodes) {
            if(GetDistanceBetweenNodesCalled != null) {
                GetDistanceBetweenNodesCalled(node1, node2, allNodes);
            }
            return Int32.MaxValue;
        }

        public override NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin, Predicate<MapNodeBase> condition, int maxDistance) {
            if(GetNearestNodeToEdgeWhereCalled != null) {
                GetNearestNodeToEdgeWhereCalled(edgeOfOrigin, condition, maxDistance);
            }
            return new NodeDistanceSummary(null, 5);
        }

        public override NodeDistanceSummary GetNearestNodeToNodeWhere(MapNodeBase rootNode, Predicate<MapNodeBase> condition, int maxDistance) {
            if(GetNearestNodeToNodeWhereCalled != null) {
                GetNearestNodeToNodeWhereCalled(rootNode, condition, maxDistance);
            }
            return new NodeDistanceSummary(null, 5);
        }

        public override List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase start, MapNodeBase end, IEnumerable<MapNodeBase> allNodes) {
            if(GetShortestPathBetweenNodesCalled != null) {
                GetShortestPathBetweenNodesCalled(start, end, allNodes);
            }
            return new List<MapNodeBase>();
        }

        #endregion

        #endregion
        
    }

}
