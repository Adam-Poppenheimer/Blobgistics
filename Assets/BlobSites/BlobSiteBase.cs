using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites {

    /// <summary>
    /// The abstract base class for all blob sites, which are were all resource blobs are created,
    /// and also where they're stored when not traveling through highways.
    /// </summary>
    public abstract class BlobSiteBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The total number of blobs that can be in this blob site.
        /// </summary>
        public abstract int TotalCapacity { get; set; }

        /// <summary>
        /// Configuration data that tells this blob site how it should organize and destroy blobs.
        /// </summary>
        public abstract BlobSiteConfigurationBase Configuration { get; set; }

        /// <summary>
        /// The total amount of space left in this blob site.
        /// </summary>
        public abstract int TotalSpaceLeft { get; }

        /// <summary>
        /// Whether the blob site can accept any additional blobs or not.
        /// </summary>
        public abstract bool IsAtCapacity { get; }

        /// <summary>
        /// The resource blobs currently within the blob site.
        /// </summary>        
        public abstract ReadOnlyCollection<ResourceBlobBase> Contents { get; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever a new blob is placed into the blob site.
        /// </summary>
        public event EventHandler<BlobEventArgs> BlobPlacedInto;
        /// <summary>
        /// Fires whenever a blob is extracted from the blob site.
        /// </summary>
        public event EventHandler<BlobEventArgs> BlobExtractedFrom;
        /// <summary>
        /// Fires whenever the entire blob site is cleared.
        /// </summary>
        public event EventHandler<EventArgs>     AllBlobsCleared;

        /// <summary>
        /// Fires the BlobPlacedInto event.
        /// </summary>
        /// <param name="blob">The blob that was just placed</param>
        protected void RaiseBlobPlacedInto   (ResourceBlobBase blob) { RaiseEvent(BlobPlacedInto,    new BlobEventArgs(blob)); }
        /// <summary>
        /// Fires the BlobExtractedFrom event.
        /// </summary>
        /// <param name="blob">The blob that was just extracted</param>
        protected void RaiseBlobExtractedFrom(ResourceBlobBase blob) { RaiseEvent(BlobExtractedFrom, new BlobEventArgs(blob)); }
        /// <summary>
        /// Fires the AllBlobsCleared event.
        /// </summary>
        protected void RaiseAllBlobsCleared  ()                      { RaiseEvent(AllBlobsCleared,       EventArgs.Empty    ); }

        /// <summary>
        /// Helper method for raising events.
        /// </summary>
        /// <typeparam name="T">The EventArgs type of the event</typeparam>
        /// <param name="handler">The event handler itself</param>
        /// <param name="e">The event args to be passed to the handler</param>
        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs {
            if(handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region instance methods

        #region from Object

        /// <inheritdoc/>
        public override string ToString() {
            return "BlobSiteBase on node " + name;
        }

        #endregion

        /// <summary>
        /// Gets the point that resources coming from a given location should arrive at.
        /// This is mostly used to determine where highway endpoints should end up.
        /// </summary>
        /// <param name="point">The point the hypothetical resources are arriving from</param>
        /// <returns>The location that those hypothetical resources should go to</returns>
        public abstract Vector3 GetPointOfConnectionFacingPoint(Vector3 point);

        /// <summary>
        /// Determines whether the given blob can be placed in this blob site.
        /// </summary>
        /// <param name="blob">The blob to consider</param>
        /// <returns>Whether or not the blob site can accept the blob</returns>
        public abstract bool CanPlaceBlobInto(ResourceBlobBase blob);
        /// <summary>
        /// Places the given blob into the blob site.
        /// </summary>
        /// <param name="blob">The blob to place into</param>
        public abstract void PlaceBlobInto   (ResourceBlobBase blob);

        /// <summary>
        /// Determines whether some blob of the given type can be placed into this blob site.
        /// </summary>
        /// <param name="type">The type of blob to consider</param>
        /// <returns>Whether a blob of such a type could be placed in the blob site</returns>
        public abstract bool CanPlaceBlobOfTypeInto(ResourceType type);

        /// <summary>
        /// Determines whether there is some extractable blob within the blob site.
        /// </summary>
        /// <returns>Whether the blob site contains an extractable blob</returns>
        public abstract bool             CanExtractAnyBlob();

        /// <summary>
        /// Extracts some extractable blob from the blob site.
        /// </summary>
        /// <returns>The blob that was extracted</returns>
        public abstract ResourceBlobBase ExtractAnyBlob();

        /// <summary>
        /// Determines whether some blob of a given type can be extracted from the blob site.
        /// </summary>
        /// <param name="type">The type of blob to extract</param>
        /// <returns>Whether it can be extracted</returns>
        public abstract bool             CanExtractBlobOfType(ResourceType type);

        /// <summary>
        /// Extracts some blob of the given type from the blob site.
        /// </summary>
        /// <param name="type">the type of blob to extract</param>
        /// <returns>the blob that was extracted</returns>
        public abstract ResourceBlobBase ExtractBlobOfType   (ResourceType type);

        /// <summary>
        /// Determines whether one can extract a specific blob from the blob site.
        /// </summary>
        /// <param name="blob">The blob to extract</param>
        /// <returns>Whether it can be extracted</returns>
        /// 
        public abstract bool CanExtractBlob(ResourceBlobBase blob);

        /// <summary>
        /// Extracts a particular blob from the blob site.
        /// </summary>
        /// <param name="blob">The blob to extract</param>
        public abstract void ExtractBlob   (ResourceBlobBase blob);

        /// <summary>
        /// Provides all of the types of blobs that can be extracted.
        /// </summary>
        /// <returns>The ResourceTypes for which <see cref="CanExtractBlobOfType(ResourceType)"/> returns true</returns>
        public abstract IEnumerable<ResourceType> GetExtractableTypes();

        /// <summary>
        /// Returns all of the blobs within the blob site that have the specified type.
        /// </summary>
        /// <param name="type">The type to match against</param>
        /// <returns>All blobs that match that type</returns>
        public abstract IEnumerable<ResourceBlobBase> GetContentsOfType(ResourceType type);

        /// <summary>
        /// Returns the number of blobs within the blob site that have the specified type.
        /// </summary>
        /// <param name="type">The type to match against</param>
        /// <returns>The number of blobs that match that type</returns>
        public abstract int GetCountOfContentsOfType(ResourceType type);

        /// <summary>
        /// Returns the amount of space left for blobs of a given type.
        /// </summary>
        /// <param name="type">The type to consider</param>
        /// <returns>The amount of space left for that type</returns>
        public abstract int GetSpaceLeftOfType(ResourceType type);

        /// <summary>
        /// Gets whether this blob site is permitting the placement of blobs of a particular type.
        /// </summary>
        /// <param name="type">The type to consider</param>
        /// <returns>Whether the blob site permits placement of that type</returns>
        public abstract bool GetPlacementPermissionForResourceType(ResourceType type);

        /// <summary>
        /// Sets the placement permission for the given resource to the given value.
        /// </summary>
        /// <param name="type">The ResourceType to change</param>
        /// <param name="isPermitted">Whether it's now permitted</param>
        public abstract void SetPlacementPermissionForResourceType(ResourceType type, bool isPermitted);

        /// <summary>
        /// Gets whether this blob site is permitting the extraction of blobs of a particular type.
        /// </summary>
        /// <param name="type">The type to consider</param>
        /// <returns>Whether the blob site permits extraction of that type</returns>
        public abstract bool GetExtractionPermissionForResourceType(ResourceType type);

        /// <summary>
        /// Sets the extraction permission for the given resource to the given value.
        /// </summary>
        /// <param name="type">The ResourceType to change</param>
        /// <param name="isPermitted">Whether it's now permitted</param>
        public abstract void SetExtractionPermissionForResourceType(ResourceType type, bool isPermitted);

        /// <summary>
        /// Gets the per-resource capacity of a particular resource type.
        /// </summary>
        /// <param name="type">The ResourceType to consider</param>
        /// <returns>Its specified capacity</returns>
        public abstract int GetCapacityForResourceType(ResourceType type);

        /// <summary>
        /// Sets the per-resource capacity of a particular resource type.
        /// </summary>
        /// <param name="type">The ResourceType to change</param>
        /// <param name="newCapacity">The new capacity</param>
        public abstract void SetCapacityForResourceType(ResourceType type, int newCapacity);

        /// <summary>
        /// Determines whether the blob site is at capacity for a given resource type.
        /// </summary>
        /// <param name="type">The ResourceType to consider</param>
        /// <returns>Whether the blob site is at capacity for that resource</returns>
        public abstract bool GetIsAtCapacityForResource(ResourceType type);

        /// <summary>
        /// Sets the placement permissions, extraction permissions, per-resource capacity,
        /// and total capacity the align with the values defined by some <see cref="IntPerResourceDictionary"/>.
        /// </summary>
        /// <param name="placementSummary">The dictionary used to configure the blob site</param>
        public abstract void SetPlacementPermissionsAndCapacity(IntPerResourceDictionary placementSummary);

        /// <summary>
        /// Clears all permissions and capacities on the blob site.
        /// </summary>
        public abstract void ClearPermissionsAndCapacity();

        /// <summary>
        /// Destroys all contents of the blob site and clears any record of them.
        /// </summary>
        public abstract void ClearContents();

        #endregion

    }

}
