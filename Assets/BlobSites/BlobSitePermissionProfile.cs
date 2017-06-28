using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    [Serializable, DataContract]
    public class BlobSitePermissionProfile {

        #region instance fields and properties

        [DataMember()]
        private Dictionary<ResourceType, bool> PlacementPermissions =
            new Dictionary<ResourceType, bool>();

        [DataMember()]
        private Dictionary<ResourceType, bool> ExtractionPermissions =
            new Dictionary<ResourceType, bool>();

        [DataMember()]
        private Dictionary<ResourceType, int> Capacities =
            new Dictionary<ResourceType, int>();

        [DataMember()] public int TotalCapacity { get; set; }

        #endregion

        #region static methods

        public static BlobSitePermissionProfile BuildFromBlobSite(BlobSiteBase site) {
            var retval = new BlobSitePermissionProfile();

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                retval.SetCapacity(resourceType, site.GetCapacityForResourceType(resourceType));
                retval.SetPlacementPermission(resourceType, site.GetPlacementPermissionForResourceType(resourceType)); 
                retval.SetExtractionPermission(resourceType, site.GetExtractionPermissionForResourceType(resourceType));
            }
            retval.SetTotalCapacity(site.TotalCapacity);

            return retval;
        }

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
