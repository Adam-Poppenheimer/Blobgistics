using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    public abstract class MapGraphAlgorithmSetBase : MonoBehaviour {

        #region instance methods

        public abstract int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2, IEnumerable<MapNodeBase> allNodes);
        public abstract List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase start, MapNodeBase end, IEnumerable<MapNodeBase> allNodes);

        public abstract NodeDistanceSummary GetNearestNodeToEdgeWhere(MapEdgeBase edgeOfOrigin, Predicate<MapNodeBase> condition, int maxDistance);

        public abstract NodeDistanceSummary GetNearestNodeToNodeWhere(MapNodeBase rootNode, Predicate<MapNodeBase> condition, int maxDistance);

        #endregion

    }

}
