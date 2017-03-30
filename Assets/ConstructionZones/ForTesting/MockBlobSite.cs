using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using UnityEngine;

namespace Assets.ConstructionZones.ForTesting {

    public class MockBlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        public override ReadOnlyCollection<ResourceBlob> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlob> contents = new List<ResourceBlob>();

        public override Vector3 EastConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsAtCapacity {
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

        public override int TotalSpaceLeft {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 WestConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        private Dictionary<ResourceType, bool> PlacementPermissions =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> ExtractionPermissions =
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, int> Capacities =
            new Dictionary<ResourceType, int>();

        #endregion

        #region instance methods

        #region from BlobSiteBase

        public override bool CanExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobInto(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void ClearContents() {
            contents.Clear();
            RaiseAllBlobsCleared();
        }

        public override void ClearPermissionsAndCapacity() {
            PlacementPermissions.Clear();
            ExtractionPermissions.Clear();
            Capacities.Clear();
        }

        public override ResourceBlob ExtractAnyBlob() {
            throw new NotImplementedException();
        }

        public override ResourceBlob ExtractBlobOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool CanExtractBlob(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override void ExtractBlob(ResourceBlob blob) {
            throw new NotImplementedException();
        }

        public override int GetCapacityForResourceType(ResourceType type) {
            int retval;
            Capacities.TryGetValue(type, out retval);
            return retval;
        }

        public override IEnumerable<ResourceBlob> GetContentsOfType(ResourceType type) {
            return contents.Where(delegate(ResourceBlob blob) {
                return blob.BlobType == type;
            });
        }

        public override int GetCountOfContentsOfType(ResourceType type) {
            int runningTotal = 0;
            foreach(var blob in contents) {
                if(blob.BlobType == type) {
                    ++runningTotal;
                }
            }
            return runningTotal;
        }

        public override IEnumerable<ResourceType> GetExtractableTypes() {
            var resourceTypesFound = new HashSet<ResourceType>();
            foreach(var blob in Contents) {
                resourceTypesFound.Add(blob.BlobType);
            }
            return resourceTypesFound;
        }

        public override bool GetExtractionPermissionForResourceType(ResourceType type) {
            bool retval;
            ExtractionPermissions.TryGetValue(type, out retval);
            return retval;
        }

        public override bool GetIsAtCapacityForResource(ResourceType type) {
            throw new NotImplementedException();
        }

        public override bool GetPlacementPermissionForResourceType(ResourceType type) {
            bool retval;
            PlacementPermissions.TryGetValue(type, out retval);
            return retval;
        }

        public override int GetSpaceLeftOfType(ResourceType type) {
            throw new NotImplementedException();
        }

        public override void PlaceBlobInto(ResourceBlob blob) {
            contents.Add(blob);
            RaiseBlobPlacedInto(blob);
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

        public override void SetPlacementPermissionsAndCapacity(ResourceSummary placementSummary) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
