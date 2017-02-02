using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    public class MapNode : MonoBehaviour {

        #region instance fields and properties

        public ReadOnlyCollection<MapNode> Neighbors {
            get { return neighbors.AsReadOnly(); }
        }
        [SerializeField] private List<MapNode> neighbors;

        #endregion

        #region instance methods

        public void AddNeighbor(MapNode newNeighbor) {
            if(!neighbors.Contains(newNeighbor)) {
                neighbors.Add(newNeighbor);
            }else {
                throw new MapGraphException("This node already considers that node as its neighbor");
            }
        }

        public bool RemoveNeighbor(MapNode oldNeighbor) {
            return neighbors.Remove(oldNeighbor);
        }

        #endregion

    }

}
