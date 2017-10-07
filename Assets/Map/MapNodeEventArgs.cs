using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Map {

    /// <summary>
    /// A simple EventArgs class that returns Nodes on events that trigger in relation to them.
    /// </summary>
    [Serializable]
    public class MapNodeEventArgs : EventArgs {

        #region instance fields and properties

        /// <summary>
        /// The Node the event triggered in relation to.
        /// </summary>
        public readonly MapNodeBase Node;

        #endregion

        #region constructors

        /// <summary>
        /// Constructs a new MapNodeEventArgs with the given node.
        /// </summary>
        /// <param name="node">The node that triggered the event</param>
        public MapNodeEventArgs(MapNodeBase node) {
            Node = node;
        }

        #endregion

    }

}
