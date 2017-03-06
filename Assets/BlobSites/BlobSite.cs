using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Blobs;
using UnityEngine;

namespace Assets.BlobSites {

    public class BlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        public override ReadOnlyCollection<ResourceBlob> Contents {
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

        public override uint TotalSpaceLeft {
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

        #region instance methods

        #region from BlobSiteBase

        public override bool CanExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override ResourceBlob ExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override ResourceBlob ExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobInto(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void PlaceBlobInto(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ResourceType> GetExtractableTypes() {
            throw new NotImplementedException();
        }

        public override uint GetCapacityForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ResourceBlob> GetContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetCapacityForResourceType(ResourceType type, uint newCapacity) {
            throw new NotImplementedException();
        }

        public override bool GetPermissionForResourceType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPermissionForResourceType(ResourceType type, bool isPermitted) {
            throw new NotImplementedException();
        }

        public override void SetPermissionsAndCapacity(ResourceSummary summary) {
            throw new NotImplementedException();
        }

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override uint GetCountOfContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override uint GetSpaceLeftOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool GetIsAtCapacityForResource(ResourceType type) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
