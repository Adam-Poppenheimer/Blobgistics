using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    [Serializable]
    public class MapNodeEventArgs : EventArgs {

        #region instance fields and properties

        public readonly MapNodeBase Node;

        #endregion

        #region constructors

        public MapNodeEventArgs(MapNodeBase node) {
            Node = node;
        }

        #endregion

    }

}
