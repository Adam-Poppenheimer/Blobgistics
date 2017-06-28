using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.ResourceDepots;
using System.Collections.ObjectModel;

namespace Assets.Core.ForTesting {

    public class MockResourceDepotFactory : ResourceDepotFactoryBase {

        #region instance fields and properties

        #region from ResourceDepotFactoryBase

        public override ReadOnlyCollection<ResourceDepotBase> ResourceDepots {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        private List<ResourceDepotBase> depots = new List<ResourceDepotBase>();

        #endregion

        #region instance methods

        #region from ResourceDepotFactoryBase

        public override ResourceDepotBase ConstructDepotAt(MapNodeBase location) {
            var newDepot = (new GameObject()).AddComponent<MockResourceDepot>();

            newDepot.location = location;

            depots.Add(newDepot);
            return newDepot;
        }

        public override void DestroyDepot(ResourceDepotBase depot) {
            depots.Remove(depot);
            DestroyImmediate(depot.gameObject);
        }

        public override ResourceDepotBase GetDepotAtLocation(MapNodeBase location) {
            return depots.Where(depot => depot.Location == location).FirstOrDefault();
        }

        public override ResourceDepotBase GetDepotOfID(int id) {
            return depots.Where(depot => depot.ID == id).FirstOrDefault();
        }

        public override bool HasDepotAtLocation(MapNodeBase location) {
            return GetDepotAtLocation(location) != null;
        }

        public override void UnsubscribeDepot(ResourceDepotBase depot) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
