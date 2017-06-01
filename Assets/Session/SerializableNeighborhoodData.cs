using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    [Serializable]
    public class SerializableNeighborhoodData {

        #region instance fields and properties

        public string Name;

        public SerializableVector3 LocalPosition;

        public List<int> ChildNodeIDs;
        public List<int> ChildEdgeIDs;

        #endregion

        #region constructors

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
