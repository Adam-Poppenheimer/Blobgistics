using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using UnityEngine;

namespace Assets.Map.ForTesting {

    public class MockBlobSite : BlobSiteBase {

        #region instance fields and properties

        #region BlobSiteBase

        public override int TotalCapacity { get; set; }

        public override BlobSiteConfigurationBase Configuration { get; set; }

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

        #endregion

        #endregion

        #region instance methods

        #region from BlobSiteBase

        public override bool CanExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlob(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobInto(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void ClearContents() {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override int GetCapacityForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ResourceBlobBase> GetContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override int GetCountOfContentsOfType(ResourceType type) {
            throw new NotImplementedException();
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

        public override Vector3 GetPointOfConnectionFacingPoint(Vector3 point) {
            throw new NotImplementedException();
        }

        public override int GetSpaceLeftOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void PlaceBlobInto(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override void SetCapacityForResourceType(ResourceType type, int newCapacity) {
            throw new NotImplementedException();
        }

        public override void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void SetPlacementPermissionsAndCapacity(IntPerResourceDictionary placementSummary) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
