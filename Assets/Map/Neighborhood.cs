using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    /// <summary>
    /// A class used to facilitate the use of prefabs in map creation at design time.
    /// </summary>
    /// <remarks>
    /// Neighborhoods were originally conceived as ways of storing chunks of maps as
    /// prefabs that could then be loaded and manipulated as cohesive objects. It became
    /// necessary to add the Neighborhood class in order to deal with the peculiarities of
    /// session saving and loading (in the Assets.Session namespace), though over time the
    /// idea of modular level design fell out of favor. It's not clear if Neighborhoods
    /// are a meaningful tool any longer.
    /// </remarks>
    [ExecuteInEditMode]
    public class Neighborhood : MonoBehaviour {

        #region instance fields and properties

        private bool SpreadParentChangeOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

        #if UNITY_EDITOR

        private void Start() {
            SpreadParentChangeToChildren();
        }

        private void Update() {
            if(SpreadParentChangeOnNextUpdate) {
                SpreadParentChangeToChildren();
                UnityEditor.PrefabUtility.DisconnectPrefabInstance(gameObject);
                SpreadParentChangeOnNextUpdate = false;
            }
        }

        private void OnTransformParentChanged() {
            SpreadParentChangeOnNextUpdate = true;
        }

        #endif

        #endregion

        /// <summary>
        /// Facilitates subscription of child MapNodeBases and MapEdgeBases, since the current
        /// methods of handling prefab instantiations only work when the ParentGraph is a direct
        /// parent of the subscribing elements.
        /// </summary>
        public void SpreadParentChangeToChildren() {
            var mapAbove = gameObject.GetComponentInParent<MapGraphBase>();
            if(mapAbove == null) {
                return;
            }

            foreach(var node in gameObject.GetComponentsInChildren<MapNodeBase>()) {
                if(!mapAbove.Nodes.Contains(node)) {
                    mapAbove.SubscribeNode(node);
                }
            }
            foreach(var edge in gameObject.GetComponentsInChildren<MapEdgeBase>()) {
                if(!mapAbove.Edges.Contains(edge)) {
                    mapAbove.SubscribeMapEdge(edge);
                }
            }
            foreach(var neighborhoodHelper in gameObject.GetComponentsInChildren<Neighborhood>()) {
                if(neighborhoodHelper != this) {
                    neighborhoodHelper.SpreadParentChangeToChildren();
                }
            }
        }

        #endregion

    }

}
