using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Depots;
using Assets.Map;

namespace Assets.ConstructionZones.ForTesting {

    public class MockResourceDepotFactory : ResourceDepotFactoryBase {

        #region events

        public event EventHandler<MapNodeEventArgs> ConstructDepotCalled;

        #endregion

        #region instance methods

        #region from ResourceDepotFactoryBase

        public override ResourceDepotBase ConstructDepotAt(MapNodeBase map) {
            if(ConstructDepotCalled != null) {
                ConstructDepotCalled(this, new MapNodeEventArgs(map));
            }
            return null;
        }

        public override void DestroyDepot(ResourceDepotBase depot) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeDepot(ResourceDepotBase depot) {
            throw new NotImplementedException();
        }

        public override ResourceDepotBase GetDepotAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override bool HasDepotAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
