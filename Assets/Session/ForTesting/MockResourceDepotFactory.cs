using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.ResourceDepots;

namespace Assets.Session.ForTesting {

    public class MockResourceDepotFactory : ResourceDepotFactoryBase {

        #region instance fields and properties

        #region from ResourceDepotFactoryBase

        public override ReadOnlyCollection<ResourceDepotBase> ResourceDepots {
            get { return resourceDepots.AsReadOnly(); }
        }
        private List<ResourceDepotBase> resourceDepots = new List<ResourceDepotBase>();

        #endregion

        #endregion

        #region instance methods

        #region from ResourceDepotFactoryBase

        public override ResourceDepotBase ConstructDepotAt(MapNodeBase location) {
            var newDepot = (new GameObject()).AddComponent<MockResourceDepot>();
            newDepot.location = location;
            resourceDepots.Add(newDepot);
            return newDepot;
        }

        public override void DestroyDepot(ResourceDepotBase depot) {
            resourceDepots.Remove(depot);
            DestroyImmediate(depot.gameObject);
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
