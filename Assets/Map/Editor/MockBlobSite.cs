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

        private Dictionary<ResourceType, bool> TypeExtractionIsPermitted =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> TypePlacementIsPermitted =
            new Dictionary<ResourceType, bool>();

        #endregion

        #region constructors

        public MockBlobSite() {
        }

        #endregion

        #region events

        #region from IBlobSite

        public event EventHandler<EventArgs> AllBlobsCleared;
        public event EventHandler<BlobEventArgs> BlobExtractedFrom;
        public event EventHandler<BlobEventArgs> BlobPlacedInto;

        #endregion

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

        public override bool CanPlaceBlobInto(ResourceBlob blob) {
            return CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            bool retval;
            TypePlacementIsPermitted.TryGetValue(type, out retval);
            return retval;
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

        public override void Clear() {
            throw new NotImplementedException();
        }

        public override uint GetCapacityForResourceType(ResourceType type) {
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

        public override IEnumerable<ResourceBlob> GetContentsOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void SetPermissionsAndCapacity(ResourceSummary summary) {
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