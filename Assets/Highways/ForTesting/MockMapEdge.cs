﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.BlobSites;
using Assets.Map;

namespace Assets.Highways.ForTesting {

    public class MockMapEdge : MapEdgeBase {

        #region instance fields and properties

        #region from MapEdgeBase

        public override MapNodeBase FirstNode {
            get { return firstNode; }
        }
        public MapNodeBase firstNode;

        public override int ID {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapGraphBase ParentGraph {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase SecondNode {
            get { return secondNode; }
        }
        public MapNodeBase secondNode;

        #endregion

        #endregion

        #region instance methods

        #region from MapEdgeBase

        public override void RefreshOrientation() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
