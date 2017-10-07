using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {
    
    /// <summary>
    /// An EventArgs class used during events involving map edges.
    /// </summary>
    [Serializable]
    public class MapEdgeEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The MapEdgeBase that triggered the event.
        /// </summary>
        public readonly MapEdgeBase Edge;

        #endregion

        #region constructors

        /// <summary>
        /// Constructs a new MapEdgeEventArgs from a given edge.
        /// </summary>
        /// <param name="edge"></param>
        public MapEdgeEventArgs(MapEdgeBase edge) {
            Edge = edge;
        }

        #endregion

    }

}
