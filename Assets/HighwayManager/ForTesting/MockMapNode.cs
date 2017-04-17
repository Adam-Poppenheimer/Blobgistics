using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobSites;
using Assets.Map;

namespace Assets.HighwayManager.ForTesting {

    public class MockMapNode : MapNodeBase {

        #region instance fields and properties

        #region from MapNodeBase

        public override BlobSiteBase BlobSite {
            get {
                if(_blobSite == null) {
                    _blobSite = gameObject.AddComponent<MockBlobSite>();
                }
                return _blobSite;
            }
        }
        private BlobSiteBase _blobSite;

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapGraphBase ManagingGraph {
            get {
                throw new NotImplementedException();
            }
        }
        public MapGraphBase managingGraph;

        public override IEnumerable<MapNodeBase> Neighbors {
            get { return managingGraph.GetNeighborsOfNode(this); }
        }

        #endregion

        #endregion

    }

}
