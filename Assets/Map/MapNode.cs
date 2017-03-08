using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class MapNode : MapNodeBase {

        #region instance fields and properties

        public override int ID {
            get { return _id; }
        }
        public void SetID(int value) {
            _id = value;
        }
        [SerializeField, HideInInspector] private int _id;

        public override MapGraphBase ManagingGraph {
            get { return _managingGraph; }
        }
        public void SetManagingGraph(MapGraphBase value) {
            _managingGraph = value;
        }
        [SerializeField, HideInInspector] private MapGraphBase _managingGraph;

        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
        }
        [SerializeField, HideInInspector] private BlobSiteBase _blobSite;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnDestroy() {
            if(ManagingGraph != null) {
                ManagingGraph.RemoveNode(this);
            }
        }

        #endregion

        #endregion

    }

}
