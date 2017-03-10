using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Depots {

    public class ResourceDepotFactory : ResourceDepotFactoryBase {

        #region instance fields and properties

        [SerializeField] private GameObject ResourceDepotPrefab;

        #endregion

        #region instance methods

        #region from ResourceDepotFactoryBase

        public override ResourceDepotBase ConstructDepot(MapNodeBase location) {
            ResourceDepot newResourceDepot = null;
            if(ResourceDepotPrefab != null) {
                var newGameObject = Instantiate<GameObject>(ResourceDepotPrefab);
                newResourceDepot = newGameObject.GetComponent<ResourceDepot>();
                if(newResourceDepot == null) {
                    throw new ResourceDepotException("ResourceDepotPrefab does not contain a ResourceDepot component on it");
                }
            }else {
                var hostingObject = new GameObject();
                newResourceDepot = hostingObject.AddComponent<ResourceDepot>();
            }
            location.BlobSite.ClearContents();
            newResourceDepot.SetLocation(location);
            newResourceDepot.Profile = ResourceDepotProfile.Empty;
            return newResourceDepot;
        }

        public override void DestroyDepot(ResourceDepotBase depot) {
            DestroyImmediate(depot.gameObject);
        }

        #endregion

        #endregion

    }

}
