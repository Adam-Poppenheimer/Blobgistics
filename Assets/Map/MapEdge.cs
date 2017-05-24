using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using Assets.BlobSites;
using Assets.Core;

namespace Assets.Map {

    [ExecuteInEditMode]
    [SelectionBase]
    public class MapEdge : MapEdgeBase {

        #region instance fields and properties

        #region from MapEdgeBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapNodeBase FirstNode {
            get { return firstNode; }
        }
        [SerializeField] private MapNodeBase firstNode;

        public override MapNodeBase SecondNode {
            get { return secondNode; }
        }
        [SerializeField] private MapNodeBase secondNode;

        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
        }
        [SerializeField] private BlobSiteBase _blobSite;

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

        public Transform DisplayComponent {
            get { return _displayComponent; }
            set { _displayComponent = value; }
        }
        [SerializeField] private Transform _displayComponent;

        private bool DestroyOnNextUpdate = false;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(firstNode != null) {
                firstNode.TransformChanged += OnEndpointTransformChanged;
            }
            if(secondNode != null) {
                secondNode.TransformChanged += OnEndpointTransformChanged;
            }
        }

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
            if(firstNode != null) {
                firstNode.TransformChanged -= OnEndpointTransformChanged;
            }
            if(secondNode != null) {
                secondNode.TransformChanged -= OnEndpointTransformChanged;
            }
            if(ParentGraph != null) {
                ParentGraph.UnsubscribeMapEdge(this);
            }
        }

        #endregion

        public void SetNodes(MapNodeBase firstNode, MapNodeBase secondNode) {
            if(this.firstNode != null) {
                this.firstNode.TransformChanged -= OnEndpointTransformChanged;
            }
            this.firstNode = firstNode;
            if(this.firstNode != null) {
                this.firstNode.TransformChanged += OnEndpointTransformChanged;
            }

            if(this.secondNode != null) {
                this.secondNode.TransformChanged -= OnEndpointTransformChanged;
            }
            this.secondNode = secondNode;
            if(this.secondNode != null) {
                this.secondNode.TransformChanged += OnEndpointTransformChanged;
            }

            RefreshOrientation();
        }

        private void RefreshOrientation() {
            if(firstNode != null && secondNode != null) {
                EdgeOrientationUtil.AlignTransformWithEndpoints(transform, firstNode.transform.position, secondNode.transform.position, false);
                EdgeOrientationUtil.AlignTransformWithEndpoints(DisplayComponent, firstNode.transform.position, secondNode.transform.position, true);
                RaiseOrientationRefreshed();
            }
        }

        private void OnEndpointTransformChanged(object sender, EventArgs e) {
            RefreshOrientation();
        }

        #endregion

    }

}
