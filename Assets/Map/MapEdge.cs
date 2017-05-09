using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.BlobSites;
using Assets.Core;

namespace Assets.Map {

    [Serializable]
    [ExecuteInEditMode]
    public class MapEdge : MapEdgeBase {

        #region instance fields and properties

        #region from MapEdgeBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapNodeBase FirstNode {
            get { return _firstNode; }
        }
        public void SetFirstNode(MapNodeBase value) {
            _firstNode = value;
        }
        [SerializeField] private MapNodeBase _firstNode;

        public override MapNodeBase SecondNode {
            get { return _secondNode; }
        }
        public void SetSecondNode(MapNodeBase value) {
            _secondNode = value;
        }
        [SerializeField] private MapNodeBase _secondNode;

        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
        }
        [SerializeField] private BlobSiteBase _blobSite;

        public override MapGraphBase ParentGraph {
            get { return _parentGraph; }
        }
        public void SetParentGraph(MapGraphBase value) {
            _parentGraph = value;
        }
        [SerializeField] private MapGraphBase _parentGraph;

        #endregion

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnDestroy() {
            if(ParentGraph != null) {
                ParentGraph.UnsubscribeDirectedEdge(this);
            }
        }

        #endregion

        #endregion

    }

}
