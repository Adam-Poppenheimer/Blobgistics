using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a neighborhood.
    /// </summary>
    [Serializable, DataContract]
    public class SerializableNeighborhoodData {

        #region instance fields and properties

        /// <summary>
        /// The name of the neighborhood.
        /// </summary>
        [DataMember()] public string Name;

        /// <summary>
        /// The local position, in terms of the MapGraph, of the neighborhood.
        /// </summary>
        [DataMember()]  public SerializableVector3 LocalPosition;

        /// <summary>
        /// The IDs of all the nodes that are children of the neighborhood.
        /// </summary>
        [DataMember()] public List<int> ChildNodeIDs;

        /// <summary>
        /// The IDs of all the edges that are children of the neighborhood.
        /// </summary>
        [DataMember()] public List<int> ChildEdgeIDs;

        #endregion

        #region constructors

        /// <summary>
        /// Initalizes the data from the given neighborhood.
        /// </summary>
        /// <param name="neighborhood">The neighborhood to pull data from</param>
        public SerializableNeighborhoodData(Neighborhood neighborhood) {
            Name = neighborhood.name;
            LocalPosition = neighborhood.transform.localPosition;

            ChildNodeIDs = new List<int>();
            foreach(var node in neighborhood.GetComponentsInChildren<MapNodeBase>()) {
                ChildNodeIDs.Add(node.ID);
            }

            ChildEdgeIDs = new List<int>();
            foreach(var edge in neighborhood.GetComponentsInChildren<MapEdgeBase>()) {
                ChildEdgeIDs.Add(edge.ID);
            }
        }

        #endregion

    }

}
