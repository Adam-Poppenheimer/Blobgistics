using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Map;

namespace Assets.BlobEngine {

    public interface IBlobSite {

        #region properties

        Transform transform { get; }

        Vector3 NorthTubeConnectionPoint { get; }
        Vector3 SouthTubeConnectionPoint { get; }
        Vector3 EastTubeConnectionPoint  { get; }
        Vector3 WestTubeConnectionPoint  { get; }

        MapNode Location { get; }

        bool AcceptsPlacement  { get; }
        bool AcceptsExtraction { get; }

        ReadOnlyBlobPile ReadOnlyBlobsWithin { get; }

        #endregion

        #region events

        event EventHandler<BlobEventArgs> BlobPlacedInto;
        event EventHandler<BlobEventArgs> BlobExtractedFrom;
        event EventHandler<EventArgs>     AllBlobsCleared;

        #endregion

        #region methods

        Vector3 GetConnectionPointInDirection(ManhattanDirection direction);

        bool CanPlaceBlobOfTypeInto(ResourceType type);
        bool CanPlaceBlobInto      (ResourceBlob blob);
        void PlaceBlobInto         (ResourceBlob blob);

        void ReservePlaceForBlob  (ResourceBlob blob);
        void UnreservePlaceForBlob(ResourceBlob blob);

        bool CanExtractAnyBlob();
        bool CanExtractBlobOfType(ResourceType type);

        ResourceBlob ExtractAnyBlob();
        ResourceBlob ExtractBlobOfType(ResourceType type);

        IEnumerable<ResourceType> GetExtractableTypes();

        void ClearAllBlobs(bool clearReservedBlobs, bool destroyClearedBlobs);

        #endregion

    }

}
