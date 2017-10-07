using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Core;

namespace Assets.Map {

    /// <summary>
    /// The standard implementation of MapEdgeBase.
    /// </summary>
    [ExecuteInEditMode]
    [SelectionBase]
    public class MapEdge : MapEdgeBase {

        #region instance fields and properties

        #region from MapEdgeBase

        /// <inheritdoc/>
        public override int ID {
            get { return GetInstanceID(); }
        }

        /// <inheritdoc/>
        public override MapNodeBase FirstNode {
            get { return firstNode; }
        }
        [SerializeField] private MapNodeBase firstNode;

        /// <inheritdoc/>
        public override MapNodeBase SecondNode {
            get { return secondNode; }
        }
        [SerializeField] private MapNodeBase secondNode;

        /// <inheritdoc/>
        public override MapGraphBase ParentGraph {
            get { return _parentGraph; }
            set {
                _parentGraph = value;
                if(_parentGraph == null) {
                    DestroyOnNextUpdate = true;
                }else {
                    DestroyOnNextUpdate = false;
                }
            }
        }
        [SerializeField] private MapGraphBase _parentGraph;

        #endregion

        /// <summary>
        /// The display component of the MapEdge, which is intended to be a child
        /// object that holds only the logic for the display of the MapEdge.
        /// </summary>
        public Transform DisplayComponent {
            get { return _displayComponent; }
            set { _displayComponent = value; }
        }
        [SerializeField] private Transform _displayComponent;

        private bool DestroyOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

        //These methods account for certain edge cases that only become relevant during design time.
        //The Start method hedges against the possibility that the MapEdge was just copied or
        //instantiated from a prefab. The OnDestroy is critical for handling design-time destruction
        // (runtime destruction is supposed to be handled through MapGraph). And the Update method
        //handles the destruction of the ParentGraph itself.
        private void Start() {
            var graphAbove = GetComponentInParent<MapGraphBase>();
            if(graphAbove != null) {
                graphAbove.SubscribeMapEdge(this);
            }
        }

        private void Update() {
            if(DestroyOnNextUpdate) {
                DestroyImmediate(gameObject);
            }
        }

        private void OnDestroy() {
            if(ParentGraph != null) {
                ParentGraph.UnsubscribeMapEdge(this);
            }
        }

        #endregion

        #region from MapEdgeBase

        /// <inheritdoc/>
        /// <remarks>
        /// In the standard implementation, refreshing the orientation means rotating
        /// the MapEdge, while rotating and scaling its display component, via 
        /// <see cref="EdgeOrientationUtil.AlignTransformWithEndpoints(Transform, Vector3, Vector3, bool)"/>.
        /// This separated method was important for a previous implementation of the game and may
        /// now be unnecessary.
        /// </remarks>
        public override void RefreshOrientation() {
            if(firstNode != null && secondNode != null) {
                EdgeOrientationUtil.AlignTransformWithEndpoints(transform, firstNode.transform.position, secondNode.transform.position, false);
                EdgeOrientationUtil.AlignTransformWithEndpoints(DisplayComponent, firstNode.transform.position, secondNode.transform.position, true);
            }
        }

        #endregion

        /// <summary>
        /// Convenience method to modify both nodes at once.
        /// </summary>
        /// <param name="firstNode">The new value for the first node</param>
        /// <param name="secondNode">The new value for the second node</param>
        public void SetNodes(MapNodeBase firstNode, MapNodeBase secondNode) {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            RefreshOrientation();
        }

        #endregion

    }

}
