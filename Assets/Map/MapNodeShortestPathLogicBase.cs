using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    public abstract class MapNodeShortestPathLogicBase {

        #region instance methods

        public abstract int GetDistanceBetweenNodes(MapNodeBase node1, MapNodeBase node2, IEnumerable<MapNodeBase> allNodes);
        public abstract List<MapNodeBase> GetShortestPathBetweenNodes(MapNodeBase start, MapNodeBase end, IEnumerable<MapNodeBase> allNodes);

        #endregion

    }

}
