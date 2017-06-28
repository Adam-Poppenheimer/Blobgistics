using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    [Serializable]
    public class MapEdgeEventArgs : EventArgs {

        #region instance fields and properties

        public readonly MapEdgeBase Edge;

        #endregion

        #region constructors

        public MapEdgeEventArgs(MapEdgeBase edge) {
            Edge = edge;
        }

        #endregion

    }

}
