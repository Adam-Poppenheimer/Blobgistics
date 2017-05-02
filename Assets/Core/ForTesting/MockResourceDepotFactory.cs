using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Map;
using Assets.ResourceDepots;

namespace Assets.Core.ForTesting {

    public class MockResourceDepotFactory : ResourceDepotFactoryBase {

        #region instance methods

        #region from ResourceDepotFactoryBase

        public override ResourceDepotBase ConstructDepotAt(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override void DestroyDepot(ResourceDepotBase depot) {
            throw new NotImplementedException();
        }

        public override ResourceDepotBase GetDepotAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override ResourceDepotBase GetDepotOfID(int id) {
            throw new NotImplementedException();
        }

        public override bool HasDepotAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeDepot(ResourceDepotBase depot) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
