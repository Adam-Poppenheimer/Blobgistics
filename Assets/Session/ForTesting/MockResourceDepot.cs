using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Map;
using Assets.ResourceDepots;

namespace Assets.Session.ForTesting {

    public class MockResourceDepot : ResourceDepotBase {

        #region instance fields and properties

        #region from ResourceDepotBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapNodeBase Location {
            get { return location; }
        }
        public MapNodeBase location;

        public override ResourceDepotProfile Profile { get; set; }

        #endregion

        #endregion

        #region instance methods

        #region from ResourceDepotBase

        public override void Clear() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
