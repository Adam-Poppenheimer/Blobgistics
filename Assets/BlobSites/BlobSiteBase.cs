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

        public abstract BlobSiteConfigurationBase Configuration { get; set; }

        public abstract int TotalSpaceLeft { get; }

        public abstract bool IsAtCapacity { get; }

        public abstract ReadOnlyCollection<ResourceBlobBase> Contents { get; }

        #endregion

        #region events

        public event EventHandler<BlobEventArgs> BlobPlacedInto;
        public event EventHandler<BlobEventArgs> BlobExtractedFrom;
        public event EventHandler<EventArgs>     AllBlobsCleared;

        protected void RaiseBlobPlacedInto   (ResourceBlobBase blob) { RaiseEvent(BlobPlacedInto,    new BlobEventArgs(blob)); }
        protected void RaiseBlobExtractedFrom(ResourceBlobBase blob) { RaiseEvent(BlobExtractedFrom, new BlobEventArgs(blob)); }
        protected void RaiseAllBlobsCleared  ()                      { RaiseEvent(AllBlobsCleared,       EventArgs.Empty    ); }

        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs {
            if(handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region instance methods

        #region from Object

        public override string ToString() {
            return "BlobSiteBase on node " + name;
        }

        #endregion

        public abstract Vector3 GetPointOfConnectionFacingPoint(Vector3 point);

        public abstract bool CanPlaceBlobInto(ResourceBlobBase blob);
        public abstract void PlaceBlobInto   (ResourceBlobBase blob);
        public abstract bool CanPlaceBlobOfTypeInto(ResourceType type);

        public abstract bool             CanExtractAnyBlob();
        public abstract ResourceBlobBase ExtractAnyBlob();

        public abstract bool             CanExtractBlobOfType(ResourceType type);
        public abstract ResourceBlobBase ExtractBlobOfType   (ResourceType type);

        public abstract bool CanExtractBlob(ResourceBlobBase blob);
        public abstract void ExtractBlob   (ResourceBlobBase blob);

        public abstract IEnumerable<ResourceType> GetExtractableTypes();

        public abstract IEnumerable<ResourceBlobBase> GetContentsOfType(ResourceType type);
        public abstract int GetCountOfContentsOfType(ResourceType type);
        public abstract int GetSpaceLeftOfType(ResourceType type);

        public abstract bool GetPlacementPermissionForResourceType(ResourceType type);
        public abstract void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted);

        public abstract bool GetExtractionPermissionForResourceType(ResourceType type);
        public abstract void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted);

        public abstract int GetCapacityForResourceType(ResourceType type);
        public abstract void SetCapacityForResourceType(ResourceType type, int newCapacity);

        public abstract bool GetIsAtCapacityForResource(ResourceType type);

        public abstract void SetPlacementPermissionsAndCapacity(IntPerResourceDictionary placementSummary);

        public abstract void ClearPermissionsAndCapacity();

        public abstract void ClearContents();

        #endregion

    }

}
