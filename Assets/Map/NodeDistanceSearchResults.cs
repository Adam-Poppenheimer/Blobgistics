using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    public class NodeDistanceSummary {

        #region instance fields and properties

        public readonly MapNodeBase Node;
        public readonly int Distance;

        #endregion

        #region constructors

        public NodeDistanceSummary(MapNodeBase node, int distance) {
            Node = node;
            Distance = distance;
        }

        #endregion

    }

}
