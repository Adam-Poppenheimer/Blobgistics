using System;
using System.Collections.Generic;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;

using UnityCustomUtilities.Extensions;
using System.Collections.ObjectModel;

namespace Assets.Map.Editor {

    internal class MockBlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get {
                throw new NotImplementedException();
            }
        }

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

        public override Vector3 EastConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }
        
        public override Vector3 WestConnectionPoint {
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

        private Dictionary<ResourceType, bool> TypeExtractionIsPermitted =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> TypePlacementIsPermitted =
            new Dictionary<ResourceType, bool>();

        #endregion

        #region instance methods

        #region from BlobSiteBase

        public override bool CanExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            bool retval;
            TypeExtractionIsPermitted.TryGetValue(type, out retval);
            return retval;
        }

        public override bool CanPlaceBlobInto(ResourceBlobBase blob) {
            return CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            bool retval;
            TypePlacementIsPermitted.TryGetValue(type, out retval);
            return retval;
        }

        public override ResourceBlobBase ExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override ResourceBlobBase ExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlob(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override void ExtractBlob(ResourceBlobBase blob) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ResourceType> GetExtractableTypes() {
            throw new NotImplementedException();
        }

        public override void PlaceBlobInto(ResourceBlobBase blob) {
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

        public override IEnumerable<ResourceBlobBase> GetContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPlacementPermissionsAndCapacity(ResourceSummary summary) {
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