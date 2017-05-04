using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.ResourceDepots;

namespace Assets.Core.ForTesting {

    public class MockResourceDepotFactory : ResourceDepotFactoryBase {

        #region instance fields and properties

        private List<ResourceDepotBase> Depots = new List<ResourceDepotBase>();

        #endregion

        #region instance methods

        #region from ResourceDepotFactoryBase

        public override ResourceDepotBase ConstructDepotAt(MapNodeBase location) {
            var newDepot = (new GameObject()).AddComponent<MockResourceDepot>();

            newDepot.location = location;

            Depots.Add(newDepot);
            return newDepot;
        }

        public override void DestroyDepot(ResourceDepotBase depot) {
            Depots.Remove(depot);
            DestroyImmediate(depot.gameObject);
        }

        public override ResourceDepotBase GetDepotAtLocation(MapNodeBase location) {
            return Depots.Where(depot => depot.Location == location).FirstOrDefault();
        }

        public override ResourceDepotBase GetDepotOfID(int id) {
            return Depots.Where(depot => depot.ID == id).FirstOrDefault();
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
