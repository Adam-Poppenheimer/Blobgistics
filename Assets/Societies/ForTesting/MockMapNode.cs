using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobSites;
using Assets.Map;

namespace Assets.Societies.ForTesting {

    public class MockMapNode : MapNodeBase {

        #region instance fields and properties

        #region from MapNodeBase

        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
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

        public override IEnumerable<MapNodeBase> Neighbors {
            get {
                throw new NotImplementedException();
            }
        }

        public override TerrainType CurrentTerrain { get; set; }

        #endregion

        #endregion

    }

}
