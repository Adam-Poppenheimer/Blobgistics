using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    [Serializable]
    public class SaveableNeighborhoodSummary {

        #region instance fields and properties

        public int ID;
        public string Name;
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;

        public ReadOnlyCollection<int> NodeIDsInNeighborhood {
            get { return nodeIDsInNeighborhood.AsReadOnly(); }
        }
        [SerializeField] private List<int> nodeIDsInNeighborhood;

        public ReadOnlyCollection<int> EdgeIDsInNeighborhood {
            get { return edgeIDsInNeighborhood.AsReadOnly(); }
        }
        [SerializeField] private List<int> edgeIDsInNeighborhood;

        #endregion

        #region constructors

        public SaveableNeighborhoodSummary(int id, string name, Vector3 localPosition, Quaternion localRotation,
            List<int> nodeIDsInNeighborhood, List<int> edgeIDsInNeighborhood) {
            ID = id;
            Name = name;
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            this.nodeIDsInNeighborhood = nodeIDsInNeighborhood;
            this.edgeIDsInNeighborhood = edgeIDsInNeighborhood;
        }

        #endregion

    }

}
