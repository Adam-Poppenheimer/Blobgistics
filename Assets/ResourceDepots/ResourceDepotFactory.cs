using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Core;
using System.Collections.ObjectModel;

namespace Assets.ResourceDepots {

    /// <summary>
    /// The standard implementation for ResourceDepotFactoryBase.
    /// </summary>
    [ExecuteInEditMode]
    public class ResourceDepotFactory : ResourceDepotFactoryBase {

        #region instance fields and properties

        #region from ResourceDepotFactoryBase

        /// <inheritdoc/>
        public override ReadOnlyCollection<ResourceDepotBase> ResourceDepots {
            get { return resourceDepots.AsReadOnly(); }
        }
        [SerializeField] private List<ResourceDepotBase> resourceDepots = new List<ResourceDepotBase>();

        #endregion

        /// <summary>
        /// The UIControl that should be given to all created resource depots.
        /// </summary>
        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        [SerializeField] private GameObject ResourceDepotPrefab;
        [SerializeField] private ResourceDepotProfile StartingProfile;        

        #endregion

        #region instance methods

        #region from ResourceDepotFactoryBase

        /// <inheritdoc/>
        public override ResourceDepotBase GetDepotOfID(int id) {
            return resourceDepots.Find(depot => depot.ID == id);
        }

        /// <inheritdoc/>
        public override ResourceDepotBase GetDepotAtLocation(MapNodeBase location) {
            return resourceDepots.Find(depot => depot.Location == location);
        }

        /// <inheritdoc/>
        public override bool HasDepotAtLocation(MapNodeBase location) {
            return resourceDepots.Exists(depot => depot.Location == location);
        }

        /// <inheritdoc/>
        public override ResourceDepotBase ConstructDepotAt(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(HasDepotAtLocation(location)) {
                throw new ResourceDepotException("Cannot construct a resource depot at the specified location: one already exists");
            }
            ResourceDepot newDepot = null;
            if(ResourceDepotPrefab != null) {
                var newGameObject = Instantiate(ResourceDepotPrefab);
                newDepot = newGameObject.GetComponent<ResourceDepot>();
                if(newDepot == null) {
                    throw new ResourceDepotException("ResourceDepotPrefab does not contain a ResourceDepot component on it");
                }
            }else {
                var hostingObject = new GameObject();
                newDepot = hostingObject.AddComponent<ResourceDepot>();
            }

            location.BlobSite.ClearContents();
            newDepot.SetLocation(location);
            newDepot.Profile = StartingProfile;
            newDepot.UIControl = UIControl;
            newDepot.ParentFactory = this;

            newDepot.transform.SetParent(location.transform, false);
            newDepot.name = "ResourceDepot at " + location.name;
            newDepot.gameObject.SetActive(true);

            resourceDepots.Add(newDepot);

            newDepot.RefreshBlobSite();
            return newDepot;
        }

        /// <inheritdoc/>
        public override void DestroyDepot(ResourceDepotBase depot) {
            UnsubscribeDepot(depot);
            DestroyImmediate(depot.gameObject);
        }

        /// <inheritdoc/>
        public override void UnsubscribeDepot(ResourceDepotBase depot) {
            resourceDepots.Remove(depot);
            depot.Location.BlobSite.ClearContents();
            depot.Location.BlobSite.ClearPermissionsAndCapacity();
        }

        #endregion

        #endregion

    }

}
