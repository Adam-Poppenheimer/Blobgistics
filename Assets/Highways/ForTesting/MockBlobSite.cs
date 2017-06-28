using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Blobs;
using Assets.Map;

using Assets.BlobSites;
using System.Collections.ObjectModel;

namespace Assets.Highways.ForTesting {

    public class MockBlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contents = new List<ResourceBlobBase>();

        public override int TotalSpaceLeft {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsAtCapacity {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobSiteConfigurationBase Configuration { get; set; }

        #endregion

        private Dictionary<ResourceType, int> Capacities =
            new Dictionary<ResourceType, int>();

        private Dictionary<ResourceType, bool> PlacementPermissions =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> ExtractionPermissions = 
            new Dictionary<ResourceType, bool>();

        #endregion

        #region constructors

        public MockBlobSite() {
        }

        #endregion

        #region instance methods

        #region from BlobSiteBase

        public override Vector3 GetPointOfConnectionFacingPoint(Vector3 point) {
            var normalizedCenterToPoint = (point - transform.position).normalized;
            if(Configuration != null) {
                return (normalizedCenterToPoint * Configuration.ConnectionCircleRadius) + transform.position;
            }else {
                return (normalizedCenterToPoint * 1 ) + transform.position;
            }
        }

        public override bool CanExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            return contents.Where(blob => blob.BlobType == type).Count() > 0 &&
                GetExtractionPermissionForResourceType(type);
        }

        public override bool CanExtractBlob(ResourceBlobBase blob) {
            return contents.Contains(blob) && CanExtractBlobOfType(blob.BlobType);
        }

        public override void ExtractBlob(ResourceBlobBase blob) {
            contents.Remove(blob);
        }

        public override bool CanPlaceBlobInto(ResourceBlobBase blob) {
            return CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            return GetPlacementPermissionForResourceType(type) &&
                GetCountOfContentsOfType(type) < GetCapacityForResourceType(type);
        }

        public override ResourceBlobBase ExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override ResourceBlobBase ExtractBlobOfType(ResourceType type) {
            var blobToRemove = contents.Where(blob => blob.BlobType == type).FirstOrDefault();
            contents.Remove(blobToRemove);
            return blobToRemove;
        }

        public override IEnumerable<ResourceType> GetExtractableTypes() {
            HashSet<ResourceType> retval = new HashSet<ResourceType>();
            foreach(var blob in contents) {
                retval.Add(blob.BlobType);
            }
            return retval;
        }

        public override void PlaceBlobInto(ResourceBlobBase blob) {
            contents.Add(blob);
        }

        public override void ClearContents() {
            throw new NotImplementedException();
        }

        public override int GetCapacityForResourceType(ResourceType type) {
            int retval;
            Capacities.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetCapacityForResourceType(ResourceType type, int newCapacity) {
            Capacities[type] = newCapacity;
        }

        public override bool GetPlacementPermissionForResourceType(ResourceType type) {
            bool retval;
            PlacementPermissions.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted) {
            PlacementPermissions[type] = isPermitted;
        }

        public override bool GetExtractionPermissionForResourceType(ResourceType type) {
            bool retval;
            ExtractionPermissions.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted) {
            ExtractionPermissions[type] = isPermitted;
        }

        public override IEnumerable<ResourceBlobBase> GetContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPlacementPermissionsAndCapacity(IntPerResourceDictionary placementSummary) {
            throw new NotImplementedException();
        }

        public override void ClearPermissionsAndCapacity() {
            throw new NotImplementedException();
        }

        public override int GetCountOfContentsOfType(ResourceType type) {
            return contents.Where(blob => blob.BlobType == type).Count();
        }

        public override int GetSpaceLeftOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool GetIsAtCapacityForResource(ResourceType type) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}