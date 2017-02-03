using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class MapNode : MonoBehaviour {

        #region instance fields and properties

        public IEnumerable<MapNode> Neighbors {
            get { return ManagingGraph.GetNeighborsOfNode(this); }
        }

        public MapGraph ManagingGraph {
            get { return _managingGraph; }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _managingGraph = value;
                }
            }
        }
        [SerializeField] private MapGraph _managingGraph;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ManagingGraph != null && !ManagingGraph.Nodes.Contains(this)) {
                ManagingGraph.SubscribeNode(this);
            }
        }

        private void OnDestroy() {
            if(ManagingGraph != null) {
                ManagingGraph.RemoveNode(this);
            }
        }

        #endregion

        #endregion

    }

}
