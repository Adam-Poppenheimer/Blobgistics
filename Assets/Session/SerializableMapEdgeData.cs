using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Map;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableMapEdgeData {

        #region instance fields and properties

        [DataMember()] public int ID;

        [DataMember()] public int FirstEndpointID;
        [DataMember()] public int SecondEndpointID;

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
