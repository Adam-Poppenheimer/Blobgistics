using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Map;
using Assets.BlobSites;

namespace Assets.HighwayManager.ForTesting {

    public class MockMapEdge : MapEdgeBase {

        #region instance fields and properties

        #region from MapEdgeBase

        public override BlobSiteBase BlobSite {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase FirstNode {
            get { return firstNode; }
        }
        public MapNodeBase firstNode;

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase SecondNode {
            get { return secondNode; }
        }
        public MapNodeBase secondNode;

        public override MapGraphBase ParentGraph {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
