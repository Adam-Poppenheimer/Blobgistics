using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using UnityEngine;

namespace Assets.HighwayManager.ForTesting {

    public class MockBlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsAtCapacity {
            get {
                throw new NotImplementedException();
            }
        }

        public override int TotalSpaceLeft {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobSiteConfigurationBase Configuration { get; set; }

        #endregion

        private Dictionary<ResourceType, bool> PlacementPermissions = 
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> ExtractionPermissions = 
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, int> Capacities = 
            new Dictionary<ResourceType, int>();

        private List<ResourceBlobBase> contents =
            new List<ResourceBlobBase>();

        #endregion

        #region instance methods

        #region from BlobSiteBase

        public override Vector3 GetPointOfConnectionFacingPoint(Vector3 point) {
            throw new NotImplementedException();
        }

        public override bool CanExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlob(ResourceBlobBase blob) {
            return CanExtractBlobOfType(blob.BlobType);
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            bool isPermitted;
            ExtractionPermissions.TryGetValue(type, out isPermitted);
            return isPermitted && (contents.Find(blob => blob.BlobType == type) != null);
        }

        public override bool CanPlaceBlobInto(ResourceBlobBase blob) {
            return CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            bool isPermitted;
            PlacementPermissions.TryGetValue(type, out isPermitted);
            return isPermitted;
        }

        public override void ClearContents() {
            contents.Clear();
        }

        public override void ClearPermissionsAndCapacity() {
            throw new NotImplementedException();
        }

        public override ResourceBlobBase ExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override void ExtractBlob(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override ResourceBlobBase ExtractBlobOfType(ResourceType type) {
            var blobToRemove = contents.Find(blob => blob.BlobType == type);
            if(blobToRemove != null) {
                contents.Remove(blobToRemove);
                return blobToRemove;
            }else {
                throw new InvalidOperationException(string.Format("Attempted to extract a blob of type {0}, but no such blob exists", type));
            }
        }

        public override int GetCapacityForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ResourceBlobBase> GetContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override int GetCountOfContentsOfType(ResourceType type) {
            return contents.Where(blob => blob.BlobType == type).Count();
        }

        public override IEnumerable<ResourceType> GetExtractableTypes() {
            throw new NotImplementedException();
        }

        public override bool GetExtractionPermissionForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool GetIsAtCapacityForResource(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool GetPlacementPermissionForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override int GetSpaceLeftOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void PlaceBlobInto(ResourceBlobBase blob) {
            contents.Add(blob);
        }

        public override void SetCapacityForResourceType(ResourceType type, int newCapacity) {
            Capacities[type] = newCapacity;
        }

        public override void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted) {
            ExtractionPermissions[type] = isPermitted;
        }

        public override void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted) {
            PlacementPermissions[type] = isPermitted;
        }

        public override void SetPlacementPermissionsAndCapacity(IntResourceSummary placementSummary) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        

        
    }

}
