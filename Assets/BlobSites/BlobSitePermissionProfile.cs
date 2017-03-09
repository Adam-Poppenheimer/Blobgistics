using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

namespace Assets.BlobSites {

    public class BlobSitePermissionProfile {

        #region instance fields and properties

        private Dictionary<ResourceType, bool> PlacementPermissions =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> ExtractionPermissions =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, int> Capacities =
            new Dictionary<ResourceType, int>();

        private int TotalCapacity;

        #endregion

        #region instance methods

        public void SetPlacementPermission(ResourceType type, bool isPermitted) {
            PlacementPermissions[type] = isPermitted;
        }

        public void SetExtractionPermission(ResourceType type, bool isPermitted) {
            ExtractionPermissions[type] = isPermitted;
        }

        public void SetCapacity(ResourceType type, int newCapacity) {
            Capacities[type] = newCapacity;
        }

        public void SetTotalCapacity(int newTotalCapacity) {
            TotalCapacity = newTotalCapacity;
        }

        public void InsertProfileIntoBlobSite(BlobSiteBase blobSite) {
            blobSite.ClearPermissionsAndCapacity();
            foreach(var permissionPair in PlacementPermissions) {
                blobSite.SetPlacementPermissionForResourceType(permissionPair.Key, permissionPair.Value);
            }
            foreach(var permissionPair in ExtractionPermissions) {
                blobSite.SetExtractionPermissionForResourceType(permissionPair.Key, permissionPair.Value);
            }
            foreach(var capacityPair in Capacities) {
                blobSite.SetCapacityForResourceType(capacityPair.Key, capacityPair.Value);
            }
            blobSite.TotalCapacity = TotalCapacity;
        }

        public void Clear() {
            PlacementPermissions.Clear();
            ExtractionPermissions.Clear();
            Capacities.Clear();
        }

        #endregion

    }

}
