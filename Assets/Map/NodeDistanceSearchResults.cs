using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    public struct NodeDistanceSearchResults {

        #region instance fields and properties

        public readonly MapNodeBase Node;
        public readonly int Distance;

        #endregion

        #region constructors

        public NodeDistanceSearchResults(MapNodeBase node, int distance) {
            Node = node;
            Distance = distance;
        }

        #endregion

    }

}
