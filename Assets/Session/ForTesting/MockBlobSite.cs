﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.BlobSites;
using UnityEngine;

namespace Assets.Session.ForTesting {

    public class MockBlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        public override int TotalCapacity { get; set; }

        public override BlobSiteConfigurationBase Configuration {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<ResourceBlobBase> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlobBase> contents = new List<ResourceBlobBase>();

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

        private Dictionary<ResourceType, int> Capacities = new Dictionary<ResourceType, int>();

        private Dictionary<ResourceType, bool> PlacementPermissions  = new Dictionary<ResourceType, bool>();
        private Dictionary<ResourceType, bool> ExtractionPermissions = new Dictionary<ResourceType, bool>();

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
            Capacities.Clear();
            PlacementPermissions.Clear();
            ExtractionPermissions.Clear();
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
            int retval;
            Capacities.TryGetValue(type, out retval);
            return retval;
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

        public override Vector3 GetPointOfConnectionFacingPoint(Vector3 point) {
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

        public override void SetPlacementPermissionsAndCapacity(IntPerResourceDictionary placementSummary) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
