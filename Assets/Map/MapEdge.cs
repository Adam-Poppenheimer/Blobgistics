using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    [Serializable]
    public class MapEdge : MapEdgeBase {

        #region instance fields and properties

        public override int ID {
            get { return _id; }
        }
        public void SetID(int value) {
            _id = value;
        }
        [SerializeField] private int _id;

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

        #endregion

    }

}
