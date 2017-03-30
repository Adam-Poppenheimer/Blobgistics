using System;
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

        public override Vector3 NorthConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 SouthConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 WestConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 EastConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ResourceBlob> Contents {
            get {
                throw new NotImplementedException();
            }
        }

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

        #endregion

        #endregion

        #region constructors

        public MockBlobSite() {
        }

        #endregion

        #region instance methods

        #region from BlobSiteBase

        public override bool CanExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlob(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override void ExtractBlob(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobInto(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            throw new NotImplementedException();
        }

        public override ResourceBlob ExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override ResourceBlob ExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ResourceType> GetExtractableTypes() {
            throw new NotImplementedException();
        }

        public override void PlaceBlobInto(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override void ClearContents() {
            throw new NotImplementedException();
        }

        public override int GetCapacityForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetCapacityForResourceType(ResourceType type, int newCapacity) {
            throw new NotImplementedException();
        }

        public override bool GetPlacementPermissionForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override bool GetExtractionPermissionForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ResourceBlob> GetContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPlacementPermissionsAndCapacity(ResourceSummary placementSummary) {
            throw new NotImplementedException();
        }

        public override void ClearPermissionsAndCapacity() {
            throw new NotImplementedException();
        }

        public override int GetCountOfContentsOfType(ResourceType type) {
            throw new NotImplementedException();
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