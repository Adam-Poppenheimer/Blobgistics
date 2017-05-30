using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Map;

namespace Assets.Session {

    [Serializable]
    public class SerializableMapEdgeData {

        #region instance fields and properties

        public int ID;

        public int FirstEndpointID;
        public int SecondEndpointID;

        #endregion

        #region constructors

        public SerializableMapEdgeData(MapEdgeBase edge) {
            ID = edge.ID;
            FirstEndpointID = edge.FirstNode.ID;
            SecondEndpointID = edge.SecondNode.ID;
        }

        #endregion

    }

}
