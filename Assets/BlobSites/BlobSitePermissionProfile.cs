using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    /// <summary>
    /// A POD class that stores BlobSite placement permission, extraction permission,
    /// per resource capacity, and total capacity in a more portable form.
    /// </summary>
    /// <remarks>
    /// This data structure can't be used to underpin blob sites because dictionaries
    /// don't serialize well, though there could be a useful refactor that reduces
    /// redundancy between the two classes.
    /// </remarks>
    [Serializable, DataContract]
    public class BlobSitePermissionProfile {

        #region static fields and properties

        /// <summary>
        /// Represents the least restrictive possible profile: one with all permissions set to true
        /// and all capacities set to int.MaxValue.
        /// </summary>
        public static BlobSitePermissionProfile AllPermissiveProfile {
            get {
                if(_allPermissiveProfile == null) {
                    _allPermissiveProfile = new BlobSitePermissionProfile();
                    foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                        _allPermissiveProfile.SetPlacementPermission(resourceType, true);
                        _allPermissiveProfile.SetExtractionPermission(resourceType, true);
                        _allPermissiveProfile.SetCapacity(resourceType, int.MaxValue);
                    }
                    _allPermissiveProfile.SetTotalCapacity(int.MaxValue);
                }
                return _allPermissiveProfile;
            }
        }
        private static BlobSitePermissionProfile _allPermissiveProfile = null;

        #endregion

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

        /// <summary>
        /// The total capacity of the profile.
        /// </summary>
        [DataMember()] public int TotalCapacity { get; set; }

        #endregion

        #region static methods

        /// <summary>
        /// Creates a permission profile out of the permissions and capacities of a specified blob site.
        /// </summary>
        /// <param name="site">The site to construct the profile with</param>
        /// <returns>The profile constructed from the site</returns>
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

        /// <summary>
        /// Changes the placement permission for the specified ResourceType to the specified value.
        /// </summary>
        /// <param name="type">The ResourceType whose permission is changing</param>
        /// <param name="isPermitted">Whether or not it is now permitted</param>
        public void SetPlacementPermission(ResourceType type, bool isPermitted) {
            PlacementPermissions[type] = isPermitted;
        }

        /// <summary>
        /// Changes the extraction permission for the specified ResourceType to the specified value.
        /// </summary>
        /// <param name="type">The ResourceType whose permission is changing</param>
        /// <param name="isPermitted">Whether or not it is now permitted</param>
        public void SetExtractionPermission(ResourceType type, bool isPermitted) {
            ExtractionPermissions[type] = isPermitted;
        }

        /// <summary>
        /// Changes the per-resource capacity for the specified ResourceType to the specified value.
        /// </summary>
        /// <param name="type">The ResourceType whose capacity is changing</param>
        /// <param name="newCapacity">That ResourceType's new capacity</param>
        public void SetCapacity(ResourceType type, int newCapacity) {
            Capacities[type] = newCapacity;
        }

        /// <summary>
        /// Modifies the total capacity.
        /// </summary>
        /// <param name="newTotalCapacity">The new total capacity of the profile</param>
        public void SetTotalCapacity(int newTotalCapacity) {
            TotalCapacity = newTotalCapacity;
        }

        /// <summary>
        /// Takes the data contained within the profile and modifies the permissions and capacities
        /// of the given blob site to match them.
        /// </summary>
        /// <param name="blobSite">The blob site to be modified</param>
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

        /// <summary>
        /// Clears all permissions and capacities in the profile.
        /// </summary>
        public void Clear() {
            PlacementPermissions.Clear();
            ExtractionPermissions.Clear();
            Capacities.Clear();
        }

        #endregion

    }

}
