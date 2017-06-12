using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class Neighborhood : MonoBehaviour {

        #region instance fields and properties

        private bool SpreadParentChangeOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

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

        #endregion

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
