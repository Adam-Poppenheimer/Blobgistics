using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    /// <summary>
    /// An entity capable of having resource blobs placed into or extracted from it,
    /// and having its placement permissions, extraction permissions, and capacity changed.
    /// </summary>
    public class BlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        /// <inheritdoc/>
        public override int TotalCapacity { get; set; }

        /// <inheritdoc/>
        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contents = new List<ResourceBlobBase>();

        /// <inheritdoc/>
        public override int TotalSpaceLeft {
            get { return TotalCapacity - contents.Count; }
        }

        /// <inheritdoc/>
        public override bool IsAtCapacity {
            get { return TotalSpaceLeft == 0; }
        }

        /// <inheritdoc/>
        public override BlobSiteConfigurationBase Configuration {
            get { return _configuration; }
            set { _configuration = value; }
        }
        [SerializeField] private BlobSiteConfigurationBase _configuration;

        #endregion
        
        private Dictionary<ResourceType, bool> PlacementPermissionsByResourceType = 
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> ExtractionPermissionsByResourceType = 
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, int> CapacitiesByResourceType =
            new Dictionary<ResourceType, int>();

        #endregion

        #region instance methods

        #region from BlobSiteBase

        /// <inheritdoc/>
        /// <remarks>
        /// This implementation returns the nearest point to the specified point on some circle,
        /// centered around the BlobSite, whose radius is defined by a BlobSiteConfigurationBase
        /// object.
        /// </remarks>
        public override Vector3 GetPointOfConnectionFacingPoint(Vector3 point) {
            var normalizedCenterToPoint = (point - transform.position).normalized;
            if(Configuration != null) {
                return (normalizedCenterToPoint * Configuration.ConnectionCircleRadius) + transform.position;
            }else {
                return (normalizedCenterToPoint * 1 ) + transform.position;
            }
        }

        /// <inheritdoc/>
        public override bool CanExtractAnyBlob() {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(CanExtractBlobOfType(resourceType)) {
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public override ResourceBlobBase ExtractAnyBlob() {
            
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(GetExtractionPermissionForResourceType(resourceType)) {

                    var blobToExtract = contents.FindLast(delegate(ResourceBlobBase blob) {
                        return blob.BlobType == resourceType;
                    });
                    if(blobToExtract != null) {
                        ExtractBlob(blobToExtract);
                        return blobToExtract;
                    }
                }
            }
            throw new BlobSiteException("Cannot extract any blob from this BlobSite");
            
        }

        /// <inheritdoc/>
        public override bool CanExtractBlobOfType(ResourceType type) {
            return GetExtractionPermissionForResourceType(type) && contents.Find(delegate(ResourceBlobBase blob) {
                return blob.BlobType == type;
            }) != null;
        }

        /// <inheritdoc/>
        public override ResourceBlobBase ExtractBlobOfType(ResourceType type) {
            if(CanExtractBlobOfType(type)) {
                var blobToExtract = contents.FindLast(delegate(ResourceBlobBase blob) {
                    return blob.BlobType == type;
                });
                ExtractBlob(blobToExtract);
                return blobToExtract;
            }else {
                throw new BlobSiteException("Cannot extract a blob of this type from this BlobSite");
            }
        }

        /// <inheritdoc/>
        public override bool CanExtractBlob(ResourceBlobBase blob) {
            return GetExtractionPermissionForResourceType(blob.BlobType) && contents.Contains(blob);
        }

        /// <inheritdoc/>
        public override void ExtractBlob(ResourceBlobBase blob) {
            if(CanExtractBlob(blob)) {
                contents.Remove(blob);

                var siteScale = transform.localScale;
                var blobScale = blob.transform.localScale;

                blob.transform.localScale = new Vector3(blobScale.x * siteScale.x, blobScale.y * siteScale.y, blobScale.z * siteScale.z);

                blob.transform.SetParent(null, true);
                
                RaiseBlobExtractedFrom(blob);
                Configuration.AlignmentStrategy.RealignBlobs(contents, transform.position, Configuration.BlobRealignmentSpeedPerSecond);
            }else {
                throw new BlobSiteException("Cannot extract this blob from this BlobSite");
            }
        }

        /// <inheritdoc/>
        public override bool CanPlaceBlobInto(ResourceBlobBase blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            return !contents.Contains(blob) && CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        /// <inheritdoc/>
        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            bool isPermitted;
            PlacementPermissionsByResourceType.TryGetValue(type, out isPermitted);
            bool hasSpecificSpaceLeft = GetSpaceLeftOfType(type) > 0;
            bool hasGeneralSpaceLeft = TotalSpaceLeft > 0;

            return isPermitted && hasSpecificSpaceLeft && hasGeneralSpaceLeft;
        }

        /// <inheritdoc/>
        public override void PlaceBlobInto(ResourceBlobBase blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            if(CanPlaceBlobInto(blob)) {
                contents.Add(blob);                
                blob.transform.SetParent(transform, true);
                Configuration.AlignmentStrategy.RealignBlobs(contents, transform.position, Configuration.BlobRealignmentSpeedPerSecond);
                RaiseBlobPlacedInto(blob);
            }else {
                throw new BlobSiteException("Cannot place this blob into this BlobSite");
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<ResourceType> GetExtractableTypes() {
            HashSet<ResourceType> retval = new HashSet<ResourceType>();
            foreach(var blob in contents) {
                if(GetExtractionPermissionForResourceType(blob.BlobType)) {
                    retval.Add(blob.BlobType);
                }
            }
            return retval;
        }

        /// <inheritdoc/>
        public override int GetCapacityForResourceType(ResourceType type) {
            int retval;
            CapacitiesByResourceType.TryGetValue(type, out retval);
            return retval;
        }

        /// <inheritdoc/>
        public override void SetCapacityForResourceType(ResourceType type, int newCapacity) {
            CapacitiesByResourceType[type] = newCapacity;
        }

        /// <inheritdoc/>
        public override bool GetPlacementPermissionForResourceType(ResourceType type) {
            bool retval;
            PlacementPermissionsByResourceType.TryGetValue(type, out retval);
            return retval;
        }

        /// <inheritdoc/>
        public override void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted) {
            PlacementPermissionsByResourceType[type] = isPermitted;
        }

        /// <inheritdoc/>
        public override bool GetExtractionPermissionForResourceType(ResourceType type) {
            bool retval;
            ExtractionPermissionsByResourceType.TryGetValue(type, out retval);
            return retval;
        }

        /// <inheritdoc/>
        public override void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted) {
            ExtractionPermissionsByResourceType[type] = isPermitted;
        }

        /// <inheritdoc/>
        public override void SetPlacementPermissionsAndCapacity(IntPerResourceDictionary placementSummary) {
            if(placementSummary == null) {
                throw new ArgumentNullException("summary");
            }
            int newTotalCapacity = 0;
            PlacementPermissionsByResourceType.Clear();
            CapacitiesByResourceType.Clear();
            foreach(var resourceType in placementSummary) {
                if(placementSummary[resourceType] > 0) {
                    PlacementPermissionsByResourceType[resourceType] = true;
                    CapacitiesByResourceType[resourceType] = placementSummary[resourceType];
                    newTotalCapacity += placementSummary[resourceType];
                }
            }
            TotalCapacity = newTotalCapacity;
        }

        /// <inheritdoc/>
        public override void ClearPermissionsAndCapacity() {
            PlacementPermissionsByResourceType.Clear();
            ExtractionPermissionsByResourceType.Clear();
            CapacitiesByResourceType.Clear();
        }

        /// <inheritdoc/>
        public override IEnumerable<ResourceBlobBase> GetContentsOfType(ResourceType type) {
            return contents.Where(delegate(ResourceBlobBase blob) {
                return blob.BlobType == type;
            });
        }

        /// <inheritdoc/>
        public override int GetCountOfContentsOfType(ResourceType type) {
            return GetContentsOfType(type).Count();
        }

        /// <inheritdoc/>
        public override int GetSpaceLeftOfType(ResourceType type) {
            int capacityForType;
            CapacitiesByResourceType.TryGetValue(type, out capacityForType);
            return capacityForType - GetCountOfContentsOfType(type);
        }

        /// <inheritdoc/>
        public override bool GetIsAtCapacityForResource(ResourceType type) {
            return GetSpaceLeftOfType(type) <= 0;
        }

        /// <inheritdoc/>
        public override void ClearContents() {
            var blobsToRemove = new List<ResourceBlobBase>(contents);
            contents.Clear();
            if(Configuration != null && Configuration.BlobFactory != null) {
                foreach(var blob in blobsToRemove) {
                    Configuration.BlobFactory.DestroyBlob(blob);
                }
            }
            RaiseAllBlobsCleared();
        }

        #endregion

        #endregion

    }

}
