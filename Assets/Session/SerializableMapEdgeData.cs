using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Map;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a map edge.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableMapEdgeData {

        #region instance fields and properties

        /// <summary>
        /// The ID of the map edge.
        /// </summary>
        [DataMember()] public int ID;

        /// <summary>
        /// The ID of the first endpoint of the map edge.
        /// </summary>
        [DataMember()] public int FirstEndpointID;

        /// <summary>
        /// The ID of the second endpoint of the map edge.
        /// </summary>
        [DataMember()] public int SecondEndpointID;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from the given edge.
        /// </summary>
        /// <param name="edge">The edge to pull data from</param>
        public SerializableMapEdgeData(MapEdgeBase edge) {
            ID = edge.ID;
            FirstEndpointID = edge.FirstNode.ID;
            SecondEndpointID = edge.SecondNode.ID;
        }

        #endregion

    }

}
