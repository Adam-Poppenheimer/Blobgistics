using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobSites;
using Assets.Map;

namespace Assets.Core.ForTesting {

    public class MockMapNode : MapNodeBase {

        #region instance fields and properties

        #region from MapNodeBase

        public override BlobSiteBase BlobSite {
            get {
                throw new NotImplementedException();
            }
        }

        public override int ID {
            get { return GetInstanceID(); }
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

        public override TerrainType CurrentTerrain {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
