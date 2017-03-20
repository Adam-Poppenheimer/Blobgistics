using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    public abstract class BlobSiteBase : MonoBehaviour {

        #region instance fields and properties

        public int TotalCapacity { get; set; }

        public IBlobAlignmentStrategy AlignmentStrategy { get; set; }

        public abstract int TotalSpaceLeft { get; }

        public abstract bool IsAtCapacity { get; }

        public abstract ReadOnlyCollection<ResourceBlob> Contents { get; }

        public abstract Vector3 NorthConnectionPoint { get; }
        public abstract Vector3 SouthConnectionPoint { get; }
        public abstract Vector3 EastConnectionPoint  { get; }
        public abstract Vector3 WestConnectionPoint  { get; }

        #endregion

        #region events

        public event EventHandler<BlobEventArgs> BlobPlacedInto;
        public event EventHandler<BlobEventArgs> BlobExtractedFrom;
        public event EventHandler<EventArgs>     AllBlobsCleared;

        protected void RaiseBlobPlacedInto   (ResourceBlob blob) { RaiseEvent(BlobPlacedInto,    new BlobEventArgs(blob)); }
        protected void RaiseBlobExtractedFrom(ResourceBlob blob) { RaiseEvent(BlobExtractedFrom, new BlobEventArgs(blob)); }
        protected void RaiseAllBlobsCleared  ()                  { RaiseEvent(AllBlobsCleared,   EventArgs.Empty        ); }

        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs {
            if(handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return "BlobSiteBase " + GetInstanceID();
        }

        #endregion

        public Vector3 GetConnectionPointInDirection(ManhattanDirection direction) {
            switch(direction) {
                case ManhattanDirection.North: return NorthConnectionPoint;
                case ManhattanDirection.South: return SouthConnectionPoint;
                case ManhattanDirection.East:  return EastConnectionPoint;
                case ManhattanDirection.West:  return WestConnectionPoint;
                default: return NorthConnectionPoint;
            }
        }

        public abstract bool CanPlaceBlobInto(ResourceBlob blob);
        public abstract void PlaceBlobInto   (ResourceBlob blob);
        public abstract bool CanPlaceBlobOfTypeInto(ResourceType type);

        public abstract bool         CanExtractAnyBlob();
        public abstract ResourceBlob ExtractAnyBlob();

        public abstract bool         CanExtractBlobOfType(ResourceType type);
        public abstract ResourceBlob ExtractBlobOfType   (ResourceType type);

        public abstract IEnumerable<ResourceType> GetExtractableTypes();

        public abstract IEnumerable<ResourceBlob> GetContentsOfType(ResourceType type);
        public abstract int GetCountOfContentsOfType(ResourceType type);
        public abstract int GetSpaceLeftOfType(ResourceType type);

        public abstract bool GetPlacementPermissionForResourceType(ResourceType type);
        public abstract void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted);

        public abstract bool GetExtractionPermissionForResourceType(ResourceType type);
        public abstract void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted);

        public abstract int GetCapacityForResourceType(ResourceType type);
        public abstract void SetCapacityForResourceType(ResourceType type, int newCapacity);

        public abstract bool GetIsAtCapacityForResource(ResourceType type);

        public abstract void SetPlacementPermissionsAndCapacity(ResourceSummary placementSummary);

        public abstract void ClearPermissionsAndCapacity();

        public abstract void ClearContents();

        #endregion

    }

}
