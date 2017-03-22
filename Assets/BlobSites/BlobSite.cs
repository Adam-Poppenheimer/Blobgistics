using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    public class BlobSite : BlobSiteBase {

        #region instance fields and properties

        #region from BlobSiteBase

        public override ReadOnlyCollection<ResourceBlob> Contents {
            get { return contents.AsReadOnly(); }
        }
        private List<ResourceBlob> contents = new List<ResourceBlob>();

        public override Vector3 NorthConnectionPoint {
            get { return PrivateData.NorthConnectionOffset + transform.position; }
        }

        public override Vector3 SouthConnectionPoint {
            get { return PrivateData.SouthConnectionOffset + transform.position; }
        }

        public override Vector3 EastConnectionPoint {
            get { return PrivateData.EastConnectionOffset + transform.position; }
        }

        public override Vector3 WestConnectionPoint {
            get { return PrivateData.WestConnectionOffset + transform.position; }
        }

        public override int TotalSpaceLeft {
            get { return TotalCapacity - contents.Count; }
        }

        public override bool IsAtCapacity {
            get { return TotalSpaceLeft == 0; }
        }

        #endregion

        public BlobSitePrivateDataBase PrivateData {
            get {
                if(_privateData == null) {
                    throw new InvalidOperationException("PrivateData is uninitialized");
                } else {
                    return _privateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                }
            }
        }
        [SerializeField] private BlobSitePrivateDataBase _privateData;
        
        private Dictionary<ResourceType, bool> PlacementPermissionsByResourceType = 
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, bool> ExtractionPermissionsByResourceType = 
            new Dictionary<ResourceType, bool>();

        private Dictionary<ResourceType, int> CapacitiesByResourceType =
            new Dictionary<ResourceType, int>();

        #endregion

        #region instance methods

        #region from BlobSiteBase

        public override bool CanExtractAnyBlob() {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(CanExtractBlobOfType(resourceType)) {
                    return true;
                }
            }
            return false;
        }

        public override ResourceBlob ExtractAnyBlob() {
            
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(GetExtractionPermissionForResourceType(resourceType)) {

                    var blobToExtract = contents.FindLast(delegate(ResourceBlob blob) {
                        return blob.BlobType == resourceType;
                    });
                    if(blobToExtract != null) {
                        contents.Remove(blobToExtract);
                        blobToExtract.transform.SetParent(null, true);
                        RaiseBlobExtractedFrom(blobToExtract);
                        return blobToExtract;
                    }

                }
            }
            throw new BlobSiteException("Cannot extract any blob from this BlobSite");
            
        }

        public override bool CanExtractBlobOfType(ResourceType type) {
            return GetExtractionPermissionForResourceType(type) && contents.Find(delegate(ResourceBlob blob) {
                return blob.BlobType == type;
            }) != null;
        }

        public override ResourceBlob ExtractBlobOfType(ResourceType type) {
            if(CanExtractBlobOfType(type)) {
                var blobToExtract = contents.FindLast(delegate(ResourceBlob blob) {
                    return blob.BlobType == type;
                });
                contents.Remove(blobToExtract);
                blobToExtract.transform.SetParent(null, true);
                RaiseBlobExtractedFrom(blobToExtract);
                return blobToExtract;
            }else {
                throw new BlobSiteException("Cannot extract a blob of this type from this BlobSite");
            }
        }

        public override bool CanPlaceBlobInto(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            return !contents.Contains(blob) && CanPlaceBlobOfTypeInto(blob.BlobType);
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type) {
            bool isPermitted;
            PlacementPermissionsByResourceType.TryGetValue(type, out isPermitted);
            bool hasSpecificSpaceLeft = GetSpaceLeftOfType(type) > 0;
            bool hasGeneralSpaceLeft = TotalSpaceLeft > 0;

            return isPermitted && hasSpecificSpaceLeft && hasGeneralSpaceLeft;
        }

        public override void PlaceBlobInto(ResourceBlob blob) {
            if(blob == null) {
                throw new ArgumentNullException("blob");
            }
            if(CanPlaceBlobInto(blob)) {
                contents.Add(blob);
                blob.transform.SetParent(transform, false);
                blob.transform.localPosition = new Vector3(0, 0, ResourceBlob.DesiredZPositionOfAllBlobs);
                RaiseBlobPlacedInto(blob);
            }else {
                throw new BlobSiteException("Cannot place this blob into this BlobSite");
            }
        }

        public override IEnumerable<ResourceType> GetExtractableTypes() {
            HashSet<ResourceType> retval = new HashSet<ResourceType>();
            foreach(var blob in contents) {
                if(GetExtractionPermissionForResourceType(blob.BlobType)) {
                    retval.Add(blob.BlobType);
                }
            }
            return retval;
        }

        public override int GetCapacityForResourceType(ResourceType type) {
            int retval;
            CapacitiesByResourceType.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetCapacityForResourceType(ResourceType type, int newCapacity) {
            CapacitiesByResourceType[type] = newCapacity;
        }

        public override bool GetPlacementPermissionForResourceType(ResourceType type) {
            bool retval;
            PlacementPermissionsByResourceType.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted) {
            PlacementPermissionsByResourceType[type] = isPermitted;
        }

        public override bool GetExtractionPermissionForResourceType(ResourceType type) {
            bool retval;
            ExtractionPermissionsByResourceType.TryGetValue(type, out retval);
            return retval;
        }

        public override void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted) {
            ExtractionPermissionsByResourceType[type] = isPermitted;
        }

        public override void SetPlacementPermissionsAndCapacity(ResourceSummary placementSummary) {
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

        public override void ClearPermissionsAndCapacity() {
            PlacementPermissionsByResourceType.Clear();
            ExtractionPermissionsByResourceType.Clear();
            CapacitiesByResourceType.Clear();
        }

        public override IEnumerable<ResourceBlob> GetContentsOfType(ResourceType type) {
            return contents.Where(delegate(ResourceBlob blob) {
                return blob.BlobType == type;
            });
        }

        public override int GetCountOfContentsOfType(ResourceType type) {
            return GetContentsOfType(type).Count();
        }

        public override int GetSpaceLeftOfType(ResourceType type) {
            int capacityForType;
            CapacitiesByResourceType.TryGetValue(type, out capacityForType);
            return capacityForType - GetCountOfContentsOfType(type);
        }

        public override bool GetIsAtCapacityForResource(ResourceType type) {
            return GetSpaceLeftOfType(type) <= 0;
        }

        public override void ClearContents() {
            contents.Clear();
            RaiseAllBlobsCleared();
        }

        #endregion

        #endregion

    }

}
